using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class EllipseRenderer
{
    public static void Render(SKCanvas canvas, EllipseShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        canvas.DrawOval(skRect, paint);
    }

    public static void RenderCircle(SKCanvas canvas, CircleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Size, shape.Size);

        var skRect = new SKRect(0, 0, shape.Size, shape.Size);
        canvas.DrawOval(skRect, paint);
    }
}
