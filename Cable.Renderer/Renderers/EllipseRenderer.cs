using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class EllipseRenderer(SKRenderPipeline pipeline)
{
    private readonly SKRenderPipeline _pipeline = pipeline;

    public void Render(SKRenderContext ctx, EllipseShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, ctx.Time, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        ctx.Canvas.DrawOval(skRect, paint);
    }

    public void RenderCircle(SKRenderContext ctx, CircleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, ctx.Time, shape.Size, shape.Size);

        var skRect = new SKRect(0, 0, shape.Size, shape.Size);
        ctx.Canvas.DrawOval(skRect, paint);
    }
}
