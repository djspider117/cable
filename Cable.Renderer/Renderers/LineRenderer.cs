using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class LineRenderer(SKRenderPipeline pipeline)
{
    private readonly SKRenderPipeline _pipeline = pipeline;

    public void Render(SKCanvas canvas, LineShape shape, IMaterial? material, Transform transform)
    {
        var sz = shape.End - shape.Start;

        using var paint = _pipeline.PaintProvider.CreatePaint(material, MathF.Abs(sz.X), MathF.Abs(sz.Y));
        paint.IsStroke = true;
        paint.StrokeWidth = shape.Thickness;

        canvas.DrawLine(shape.Start.ToSKPoint(), shape.End.ToSKPoint(), paint);
    }
}