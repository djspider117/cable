using SkiaSharp;

namespace Cable.Renderer;

public readonly struct SKRenderContext(SKCanvas canvas, float time)
{
    public readonly SKCanvas Canvas = canvas;
    public readonly float Time = time;
}