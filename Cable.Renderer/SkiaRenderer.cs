using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Collections.Concurrent;
using System.Numerics;

namespace Cable.Renderer;

public class SkiaRenderer
{
    public event EventHandler<Vector2?>? DesiredSizeChanged;

    private readonly RendererProvider rendererProvider = new();
    private float _aa;
    private Camera2D _camera;
    private ConcurrentQueue<RasterizerData> _renderQueue = new();

    public Vector2? DesiredSize { get; private set; }

    public void SetSize(Vector2 desiredSize)
    {
        DesiredSize = desiredSize;
        DesiredSizeChanged?.Invoke(this, desiredSize);
    }

    public void PushFrame(RasterizerData rasterizerData)
    {
        _renderQueue.Enqueue(rasterizerData);
    }

    public void Render(SKPaintSurfaceEventArgs e)
    {
        if (!_renderQueue.TryDequeue(out var frameData))
            return;

        Render(e, frameData);
    }

    public void Render(SKPaintSurfaceEventArgs e, RasterizerData renderData)
    {
        _aa = renderData.AA;
        _camera = renderData.Camera;

        var canvas = e.Surface.Canvas;

        canvas.Clear(SKColors.Black);

        foreach (var element in renderData.Elements)
        {
            Render(e, element);
        }
    }

    private void Render(SKPaintSurfaceEventArgs e, RenderableElement elem)
    {
        if (elem.Shape is ShapeCollection col)
        {
            foreach (var shape in col)
            {
                Render(e, shape, elem.Material, elem.Transform);
            }
        }
        else if (elem.Shape != null)
        {
            Render(e, elem.Shape, elem.Material, elem.Transform);
        }
    }

    private void Render(SKPaintSurfaceEventArgs e, IShape shape, IMaterial? material, Transform transform)
    {
        var renderingFunction = rendererProvider.GetRenderFunction(shape);
        renderingFunction(e, shape, material, transform);
    }
}
