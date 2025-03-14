using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Cable.Renderer;

public class RectangleRenderer
{
    public static void Render(SKPaintSurfaceEventArgs e, RectangleShape shape, IMaterial? material, Transform transform)
    {
        var canvas = e.Surface.Canvas;

        using var paint = SKPaintProvider.CreatePaint(material, shape.Width, shape.Height);

        canvas.Save();
        canvas.Translate(transform.Translate.X, transform.Translate.Y);
        canvas.RotateDegrees(transform.Rotation);
        canvas.Scale(transform.Scale.X, transform.Scale.Y);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        canvas.DrawRect(skRect, paint);
        canvas.Restore();
    }
}
