using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Cable.Renderer;

public class EllipseRenderer
{
    public static void Render(SKPaintSurfaceEventArgs e, EllipseShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        e.Surface.Canvas.DrawOval(skRect, paint);
    }

    public static void RenderCircle(SKPaintSurfaceEventArgs e, CircleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Size, shape.Size);

        var skRect = new SKRect(0, 0, shape.Size, shape.Size);
        e.Surface.Canvas.DrawOval(skRect, paint);
    }
}
