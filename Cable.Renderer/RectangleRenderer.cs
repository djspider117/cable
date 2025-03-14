using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Cable.Renderer;

public class RectangleRenderer
{
    public static void Render(SKCanvas canvas, RectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        canvas.DrawRect(skRect, paint);
    }

    public static void RenderRounded(SKCanvas canvas, RoundedRectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        var roundedRect = new SKRoundRect(skRect, shape.Radius);
        canvas.DrawRoundRect(roundedRect, paint);
    }
}