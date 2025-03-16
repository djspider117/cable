using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class RectangleRenderer(SKRenderPipeline pipeline)
{
    private readonly SKRenderPipeline _pipeline = pipeline;
    public void Render(SKRenderContext ctx, RectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, ctx.Time, shape.Width, shape.Height);
        
        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        ctx.Canvas.DrawRect(skRect, paint);
    }

    public void RenderRounded(SKRenderContext ctx, RoundedRectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, ctx.Time, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        var roundedRect = new SKRoundRect(skRect, shape.Radius);
        ctx.Canvas.DrawRoundRect(roundedRect, paint);
    }
}