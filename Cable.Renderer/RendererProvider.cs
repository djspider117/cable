using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Media.Media3D;

namespace Cable.Renderer;

public class RendererProvider
{
    public Action<SKPaintSurfaceEventArgs, IShape, IMaterial?, Transform> GetRenderFunction(IShape shape)
    {
        if (shape is RectangleShape rectangleShape)
            return (e, shape, mat, transform) => RectangleRenderer.Render(e, rectangleShape, mat, transform);

        throw new ArgumentException(nameof(shape));
    }
}
