using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Cable.Renderer;

public class RectangleRenderer
{
    public static void Render(SKPaintSurfaceEventArgs e, RectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        e.Surface.Canvas.DrawRect(skRect, paint);
    }

    public static void RenderRounded(SKPaintSurfaceEventArgs e, RoundedRectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        var roundedRect = new SKRoundRect(skRect, shape.Radius);
        e.Surface.Canvas.DrawRoundRect(roundedRect, paint);
    }
}