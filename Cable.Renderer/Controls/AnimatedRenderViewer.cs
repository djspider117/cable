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
    private bool _ignorePixelScaling;

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

    public SkiaRenderer? Renderer { get; set; }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        if (Renderer == null)
            return;

        if (Visibility != Visibility.Visible || PresentationSource.FromVisual(this) == null)
            return;

        var size = CreateSize(out var unscaledSize, out var scaleX, out var scaleY);
        var userVisibleSize = IgnorePixelScaling ? unscaledSize : size;

        CanvasSize = userVisibleSize;

        if (size.Width <= 0 || size.Height <= 0)
            return;

        var info = new SKImageInfo(size.Width, size.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

        if (_bitmap == null || info.Width != _bitmap.PixelWidth || info.Height != _bitmap.PixelHeight)
        {
            _bitmap = new WriteableBitmap(info.Width, size.Height, BITMAP_DPI * scaleX, BITMAP_DPI * scaleY, PixelFormats.Pbgra32, null);
        }

        _bitmap.Lock();
        using (var surface = SKSurface.Create(info, _bitmap.BackBuffer, _bitmap.BackBufferStride))
        {
            if (IgnorePixelScaling)
            {
                var canvas = surface.Canvas;
                canvas.Scale(scaleX, scaleY);
                canvas.Save();
            }

            var e = new SKPaintSurfaceEventArgs(surface, info.WithSize(userVisibleSize), info);
            Renderer.Render(e);
        }

        _bitmap.AddDirtyRect(new Int32Rect(0, 0, info.Width, size.Height));
        _bitmap.Unlock();
        drawingContext.DrawImage(_bitmap, new Rect(0, 0, ActualWidth, ActualHeight));
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

        var m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
        scaleX = (float)m.M11;
        scaleY = (float)m.M22;
        return new SKSizeI((int)(w * scaleX), (int)(h * scaleY));

        static bool IsPositive(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
        }
    }
}
