using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer.Renderers;

public class EllipseRenderer(SKRenderPipeline pipeline)
{
    private readonly SKRenderPipeline _pipeline = pipeline;

    public void Render(SKCanvas canvas, EllipseShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, shape.Width, shape.Height);

        var skRect = new SKRect(0, 0, shape.Width, shape.Height);
        canvas.DrawOval(skRect, paint);
    }

    public void RenderCircle(SKCanvas canvas, CircleShape shape, IMaterial? material, Transform transform)
    {
        using var paint = _pipeline.PaintProvider.CreatePaint(material, shape.Size, shape.Size);

        var skRect = new SKRect(0, 0, shape.Size, shape.Size);
        canvas.DrawOval(skRect, paint);
    }
}
