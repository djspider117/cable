using Cable.Data.Types;
using Cable.Renderer.Renderers;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Media.Media3D;

namespace Cable.Renderer;

public class RendererProvider
{
    private readonly RectangleRenderer _rectRenderer;
    private readonly EllipseRenderer _elliRenderer;
    private readonly LineRenderer _lineRenderer;

    public RendererProvider(SKRenderPipeline pipeline)
    {
        _rectRenderer = new RectangleRenderer(pipeline);
        _lineRenderer = new LineRenderer(pipeline);
        _elliRenderer = new EllipseRenderer(pipeline);
    }

    public Action<SKCanvas, IShape, IMaterial?, Transform> GetRenderFunction(IShape shape)
    {
        if (shape is RectangleShape rectangleShape)
            return (e, shape, mat, transform) => _rectRenderer.Render(e, rectangleShape, mat, transform);

        if (shape is RoundedRectangleShape roundedRectangleShape)
            return (e, shape, mat, transform) => _rectRenderer.RenderRounded(e, roundedRectangleShape, mat, transform);

        if (shape is EllipseShape es)
            return (e, shape, mat, transform) => _elliRenderer.Render(e, es, mat, transform);

        if (shape is CircleShape cs)
            return (e, shape, mat, transform) => _elliRenderer.RenderCircle(e, cs, mat, transform);

        if (shape is LineShape ls)
            return (e, shape, mat, transform) => _lineRenderer.Render(e, ls, mat, transform);

        throw new ArgumentException(null, nameof(shape));
    }
}
