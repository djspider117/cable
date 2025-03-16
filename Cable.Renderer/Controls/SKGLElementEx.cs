using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Cable.Renderer.Controls;
[DefaultEvent("PaintSurface")]
[DefaultProperty("Name")]
public class SKGLElementEx : GLWpfControl, IDisposable
{
    private const SKColorType COLOR_TYPE = SKColorType.Rgba8888;
    private const GRSurfaceOrigin SURFACE_ORIGIN = GRSurfaceOrigin.BottomLeft;

    private bool _designMode;
    private GRContext _grContext;
    private GRGlFramebufferInfo _glInfo;
    private GRBackendRenderTarget _renderTarget;
    private SKSurface _surface;
    private SKCanvas _canvas;
    private SKSizeI _lastSize;
    private bool _disposed;
    private System.Numerics.Vector2? _desiredCanvasSize;

    public System.Numerics.Vector2? DesiredCanvasSize { get => _desiredCanvasSize; set => _desiredCanvasSize = value; }
    public GRContext GRContext => _grContext;

    [Category("Appearance")]
    public event EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

    public SKGLElementEx()
    {
        Initialize();
    }

    private void Initialize()
    {
        _designMode = DesignerProperties.GetIsInDesignMode(this);
        var settings = new GLWpfControlSettings
        {
            MajorVersion = 2,
            MinorVersion = 1,
            RenderContinuously = false
        };
        Render += OnPaint;
        Loaded += SKGLElement_Loaded;
        Unloaded += SKGLElement_Unloaded;
        RegisterToEventsDirectly = false;
        Start(settings);
    }

    private void SKGLElement_Unloaded(object sender, RoutedEventArgs e)
    {
        Release();
    }

    private void SKGLElement_Loaded(object sender, RoutedEventArgs e)
    {
        InvalidateVisual();
    }

    private SKSizeI GetSize()
    {
        if (_desiredCanvasSize != null)
            return new SKSizeI((int)_desiredCanvasSize.Value.X, (int)_desiredCanvasSize.Value.Y);

        double num = base.ActualWidth;
        double num2 = base.ActualHeight;
        if (num < 0.0 || num2 < 0.0)
        {
            num = 0.0;
            num2 = 0.0;
        }

        var presentationSource = PresentationSource.FromVisual(this);
        double num3 = 1.0;
        double num4 = 1.0;
        if (presentationSource != null)
        {
            num3 = presentationSource.CompositionTarget.TransformToDevice.M11;
            num4 = presentationSource.CompositionTarget.TransformToDevice.M22;
        }

        return new SKSizeI((int)(num * num3), (int)(num2 * num4));
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        _grContext?.ResetContext();

        base.OnRender(drawingContext);
    }

    protected virtual void OnPaint(TimeSpan e)
    {
        if (_disposed || _designMode)
        {
            return;
        }

        if (_grContext == null)
        {
            GRGlInterface backendContext = GRGlInterface.Create();
            _grContext = GRContext.CreateGl(backendContext);
        }

        SKSizeI size = GetSize();
        GL.ClearColor(Color4.Transparent);
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.ColorBufferBit);
        if (_renderTarget == null || _lastSize != size || !_renderTarget.IsValid)
        {
            _lastSize = size;
            GL.GetInteger(GetPName.DrawFramebufferBinding, out var data);
            GL.GetInteger(GetPName.StencilBits, out var data2);
            GL.GetInteger(GetPName.Samples, out var data3);
            int maxSurfaceSampleCount = _grContext.GetMaxSurfaceSampleCount(SKColorType.Rgba8888);
            if (data3 > maxSurfaceSampleCount)
            {
                data3 = maxSurfaceSampleCount;
            }

            _glInfo = new GRGlFramebufferInfo((uint)data, SKColorType.Rgba8888.ToGlSizedFormat());
            _surface?.Dispose();
            _surface = null;
            _canvas = null;
            _renderTarget?.Dispose();
            _renderTarget = new GRBackendRenderTarget(size.Width, size.Height, data3, data2, _glInfo);
        }

        if (_surface == null)
        {
            _surface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            _canvas = _surface.Canvas;
        }

        using (new SKAutoCanvasRestore(_canvas, doSave: true))
        {
            OnPaintSurface(new SKPaintGLSurfaceEventArgs(_surface, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888));
        }

        _canvas.Flush();
    }

    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        PaintSurface?.Invoke(this, e);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Release();
            _disposed = true;
        }
    }

    private void Release()
    {
        _canvas = null;
        _surface?.Dispose();
        _surface = null;
        _renderTarget?.Dispose();
        _renderTarget = null;
        _grContext?.Dispose();
        _grContext = null;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}