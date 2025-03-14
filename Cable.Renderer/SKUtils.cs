using SkiaSharp;

namespace Cable.Renderer;

public static class SKUtils
{
    public static SKColor ColorLerp(SKColor color1, SKColor color2, float t)
    {
        return new SKColor(
            (byte)(color1.Red + ((color2.Red - color1.Red) * t)),
            (byte)(color1.Green + ((color2.Green - color1.Green) * t)),
            (byte)(color1.Blue + ((color2.Blue - color1.Blue) * t)),
            (byte)(color1.Alpha + ((color2.Alpha - color1.Alpha) * t)));
    }
}
