using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Windows.ApplicationModel;
using System.Numerics;

namespace Cable.Renderer.Controls;
public class AnimatedRenderViewer : FrameworkElement
{
    public const double BITMAP_DPI = 96.0;

    private WriteableBitmap? _bitmap;
    private nint _backBuffer;
    private int _backBufferStride;
    private bool _ignorePixelScaling;
    private bool _isRendering;
    private Thread _renderingThread;
    private Matrix _transformToDevice = Matrix.Identity;

    public Vector2 ComputedSize => Renderer?.DesiredSize != null ? Renderer.DesiredSize.Value : new Vector2((float)ActualWidth, (float)ActualHeight);
    public SKSize CanvasSize { get; private set; }
    public bool IgnorePixelScaling
    {
        get => Renderer != null && _ignorePixelScaling;
        set
        {
            _ignorePixelScaling = value;
            InvalidateVisual();
        }
    }

    public SKRenderer? Renderer { get; set; }

    public AnimatedRenderViewer()
    {
        Loaded += AnimatedRenderViewer_Loaded;
        Unloaded += AnimatedRenderViewer_Unloaded;
        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void AnimatedRenderViewer_Loaded(object sender, RoutedEventArgs e)
    {
        _isRendering = true;
        Task.Run(RenderContent);

    }

    private void AnimatedRenderViewer_Unloaded(object sender, RoutedEventArgs e)
    {
        Unloaded -= AnimatedRenderViewer_Unloaded;
        _isRendering = false;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }

    private bool _renderingContent;
    private SKImageInfo _info;
    private float _scaleX;
    private float _scaleY;
    private SKSizeI _userVisibleSize;
    private readonly ManualResetEventSlim _hEvent = new(true);

    private void RenderContent()
    {
        while (_isRendering)
        {
            if (Renderer == null)
                return;

            _renderingContent = true;

            _hEvent.Wait();

            using (var surface = SKSurface.Create(_info, _backBuffer, _backBufferStride))
            {
                if (IgnorePixelScaling)
                {
                    var canvas = surface.Canvas;
                    canvas.Scale(_scaleX, _scaleY);
                    canvas.Save();
                }

                var e = new SKPaintSurfaceEventArgs(surface, _info.WithSize(_userVisibleSize), _info);

                Renderer.Render(e);
            }

            var w = _info.Width;
            var h = _info.Height;
            _renderingContent = false;

            _hEvent.Reset();
            Thread.Sleep(16);
        }
    }

    private bool _isBitmapLocked = false;
    private WriteableBitmap _lastRenderedFrame;

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (!_hEvent.IsSet && _isBitmapLocked)
        {
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _info.Width, _info.Height));
            _bitmap.Unlock();

            _lastRenderedFrame = _bitmap.Clone();
            _lastRenderedFrame.Freeze();

            drawingContext.DrawImage(_bitmap, new Rect(0, 0, ActualWidth, ActualHeight));
            _isBitmapLocked = false;
            return;
        }

        if (Visibility != Visibility.Visible || PresentationSource.FromVisual(this) == null)
            return;

        _transformToDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;

        var size = CreateSize(out var unscaledSize, out var scaleX, out var scaleY);
        _scaleX = scaleX;
        _scaleY = scaleY;
        _userVisibleSize = IgnorePixelScaling ? unscaledSize : size;

        CanvasSize = _userVisibleSize;

        if (size.Width <= 0 || size.Height <= 0)
            return;

        _info = new SKImageInfo(size.Width, size.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

        if (!_renderingContent)
        {
            if (_bitmap == null || _info.Width != _bitmap.PixelWidth || _info.Height != _bitmap.PixelHeight)
            {
                _bitmap = new WriteableBitmap(_info.Width, size.Height, BITMAP_DPI * scaleX, BITMAP_DPI * scaleY, PixelFormats.Pbgra32, null);
                _backBuffer = _bitmap.BackBuffer;
                _backBufferStride = _bitmap.BackBufferStride;
            }
        }

        if (!_isBitmapLocked)
        {
            _bitmap.Lock();
            _isBitmapLocked = true;
        }
        _hEvent.Set();


        drawingContext.DrawImage(_lastRenderedFrame, new Rect(0, 0, ActualWidth, ActualHeight));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        InvalidateVisual();
    }

    private SKSizeI CreateSize(out SKSizeI unscaledSize, out float scaleX, out float scaleY)
    {
        unscaledSize = SKSizeI.Empty;
        scaleX = 1.0f;
        scaleY = 1.0f;

        var cs = ComputedSize;
        var w = cs.X;
        var h = cs.Y;

        if (!IsPositive(w) || !IsPositive(h))
            return SKSizeI.Empty;

        unscaledSize = new SKSizeI((int)w, (int)h);

        var m = _transformToDevice;
        scaleX = (float)m.M11;
        scaleY = (float)m.M22;
        return new SKSizeI((int)(w * scaleX), (int)(h * scaleY));

        static bool IsPositive(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
        }
    }
}
