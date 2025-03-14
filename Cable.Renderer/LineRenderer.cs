using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer;

public class LineRenderer
{
    public static void Render(SKCanvas canvas, LineShape shape, IMaterial? material, Transform transform)
    {
        var sz = shape.End - shape.Start;

        using var paint = SKPaintProvider.CreatePaint(material, MathF.Abs(sz.X), MathF.Abs(sz.Y));
        paint.IsStroke = true;
        paint.StrokeWidth = shape.Thickness;

        canvas.DrawLine(shape.Start.ToSKPoint(), shape.End.ToSKPoint(), paint);
    }
}