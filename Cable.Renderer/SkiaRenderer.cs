using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Cable.Renderer;

public class SkiaRenderer
{
    private readonly RendererProvider rendererProvider = new();
    private float _aa;
    private Camera2D _camera;

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
