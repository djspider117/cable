using Cable.Data.Types;
using SkiaSharp;

namespace Cable.Renderer;

public class SKPaintProvider
{
    public static SKPaint CreatePaint(IMaterial? material, float width = 1, float height = 1)
    {
        if (material is null)
        {
            return new SKPaint()
            {
                Color = new SKColor(255, 0, 255),
                Style = SKPaintStyle.Fill
            };
        }

        if (material is ColorMaterialData colorData)
        {
            return new SKPaint()
            {
                Color = new SKColor(
                   (byte)(colorData.Color.X * 255),
                   (byte)(colorData.Color.Y * 255),
                   (byte)(colorData.Color.Z * 255),
                   (byte)(colorData.Color.W * 255)),
                IsAntialias = true,
                Style = colorData.MaterialOptions.ToSKPaintStyle(),
                IsStroke = colorData.MaterialOptions.ApplyBorder,
                StrokeWidth = colorData.MaterialOptions.BorderThickness
            };
        }

        if (material is GradientMaterialData gradientData)
        {
            var paint = new SKPaint
            {
                IsAntialias = true,
                Style = gradientData.MaterialOptions.ToSKPaintStyle(),
                IsStroke = gradientData.MaterialOptions.ApplyBorder,
                StrokeWidth = gradientData.MaterialOptions.BorderThickness
            };

            var color1 = new SKColor(
                (byte)(gradientData.Color1.X * 255),
                (byte)(gradientData.Color1.Y * 255),
                (byte)(gradientData.Color1.Z * 255),
                (byte)(gradientData.Color1.W * 255));

            var color2 = new SKColor(
                (byte)(gradientData.Color2.X * 255),
                (byte)(gradientData.Color2.Y * 255),
                (byte)(gradientData.Color2.Z * 255),
                (byte)(gradientData.Color2.W * 255));

            SKShader shader;
            shader = gradientData.Type switch
            {
                GradientMaterialData.GradientMaterialType.Horizontal => SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(width, 0), [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Vertical => SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(0, height), [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Radial => SKShader.CreateRadialGradient(new SKPoint(0.5f * width, 0.5f * height), 0.5f * MathF.Max(width, height), [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Angular => SKShader.CreateSweepGradient(new SKPoint(0.5f * width, 0.5f * height), [color1, color2], null),
                _ => throw new InvalidOperationException("Unsupported gradient type."),
            };

            // Apply the shader to the paint
            paint.Shader = shader;

            // Handle stepped gradient if needed
            if (gradientData.RenderMode == GradientMaterialData.GradientRenderMode.Stepped)
            {
                // Create a stepped gradient effect using a color filter
                // This is a simplified approach and may need refinement
                var colors = new SKColor[gradientData.Steps];
                var positions = new float[gradientData.Steps];
                for (int i = 0; i < gradientData.Steps; i++)
                {
                    float t = i / (float)(gradientData.Steps - 1);
                    colors[i] = SKUtils.ColorLerp(color1, color2, t);
                    positions[i] = t;
                }

                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(width, 0),
                    colors,
                    positions,
                    SKShaderTileMode.Clamp);
            }

            return paint;
        }

        return new SKPaint() { Color = SKColors.White };
    }
}
