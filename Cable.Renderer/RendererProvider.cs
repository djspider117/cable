using Cable.Data.Types;
using Cable.Renderer.Renderers;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Media.Media3D;

namespace Cable.Renderer;

public class RendererProvider
{
    public Action<SKCanvas, IShape, IMaterial?, Transform> GetRenderFunction(IShape shape)
    {
        if (shape is RectangleShape rectangleShape)
            return (e, shape, mat, transform) => RectangleRenderer.Render(e, rectangleShape, mat, transform);

        if (shape is RoundedRectangleShape roundedRectangleShape)
            return (e, shape, mat, transform) => RectangleRenderer.RenderRounded(e, roundedRectangleShape, mat, transform);

        if (shape is EllipseShape es)
            return (e, shape, mat, transform) => EllipseRenderer.Render(e, es, mat, transform);

        if (shape is CircleShape cs)
            return (e, shape, mat, transform) => EllipseRenderer.RenderCircle(e, cs, mat, transform);

        if (shape is LineShape ls)
            return (e, shape, mat, transform) => LineRenderer.Render(e, ls, mat, transform);

        throw new ArgumentException(null, nameof(shape));
    }
}
