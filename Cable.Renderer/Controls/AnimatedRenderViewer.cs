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
using System.Windows.Controls;
using SkiaSharp.Views.WPF;


namespace Cable.Renderer.Controls;
public class AnimatedRenderViewer : ContentControl
{
    public const double BITMAP_DPI = 96.0;

    private bool _ignorePixelScaling;
    private SKGLElementEx? _skiaElement;

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

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void AnimatedRenderViewer_Loaded(object sender, RoutedEventArgs e)
    {
        _skiaElement = new SKGLElementEx();
        _skiaElement.PaintSurface += SkiaElement_PaintSurface;
        _skiaElement.DesiredCanvasSize = Renderer?.DesiredSize;
        Content = _skiaElement;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        _skiaElement?.InvalidateVisual();
    }

    private void SkiaElement_PaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        Renderer?.SetCurrentFrameInfo(e.Info);
        Renderer?.SetCurrentSurface(e.Surface);
        Renderer?.Render(e.Surface.Canvas);
    }

}
