using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class RectangleRenderer(SKRenderPipeline pipeline)
{
    private readonly SKRenderPipeline _pipeline = pipeline;
    public void Render(SKCanvas canvas, RectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, shape.Width, shape.Height);
        
        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        canvas.DrawRect(skRect, paint);
    }

    public void RenderRounded(SKCanvas canvas, RoundedRectangleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        var roundedRect = new SKRoundRect(skRect, shape.Radius);
        canvas.DrawRoundRect(roundedRect, paint);
    }
}