using Cable.Data.Types;
using Cable.Renderer.Renderers;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Media.Media3D;

namespace Cable.Renderer;

public class SKRenderDispatcher
{
    private readonly RectangleRenderer _rectRenderer;
    private readonly EllipseRenderer _elliRenderer;
    private readonly LineRenderer _lineRenderer;

    public SKRenderDispatcher(SKRenderPipeline pipeline)
    {
        _rectRenderer = new RectangleRenderer(pipeline);
        _lineRenderer = new LineRenderer(pipeline);
        _elliRenderer = new EllipseRenderer(pipeline);
    }

    public void Render(SKRenderContext context, IShape shape, IMaterial? material, Transform transform)
    {
        if (shape is RectangleShape rectangleShape)
            _rectRenderer.Render(context, rectangleShape, material, transform);

        if (shape is RoundedRectangleShape roundedRectangleShape)
            _rectRenderer.RenderRounded(context, roundedRectangleShape, material, transform);

        if (shape is EllipseShape es)
            _elliRenderer.Render(context, es, material, transform);

        if (shape is CircleShape cs)
            _elliRenderer.RenderCircle(context, cs, material, transform);

        if (shape is LineShape ls)
            _lineRenderer.Render(context, ls, material, transform);
    }
}
