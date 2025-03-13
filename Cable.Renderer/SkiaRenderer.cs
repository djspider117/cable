using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cable.Renderer;

public class SkiaRenderer
{
    private readonly Mesh2DRenderer _meshRenderer = new();

    public void Render(SKPaintSurfaceEventArgs e, RenderCommandList commandList)
    {
        var canvas = e.Surface.Canvas;

        canvas.Clear(SKColors.Black);

        //// draw some text
        //using var paint = new SKPaint
        //{
        //    Color = SKColors.Black,
        //    IsAntialias = true,
        //    Style = SKPaintStyle.Fill
        //};
        //using var font = new SKFont
        //{
        //    Size = 24
        //};
        //var coord = new SKPoint(e.Info.Width / 2, (e.Info.Height + font.Size) / 2);
        //canvas.DrawText("SkiaSharp", coord, SKTextAlign.Center, font, paint);

        foreach (var command in commandList.Commands)
        {
            Render(e, command);
        }
    }

    public void Render(SKPaintSurfaceEventArgs e, IRenderCommand command)
    {
        if (command is Mesh2DRenderCommand meshCommand)
        {
            _meshRenderer.Render(e, meshCommand);
        }
    }
}

public class Mesh2DRenderer
{
    public void Render(SKPaintSurfaceEventArgs e, Mesh2DRenderCommand command)
    {
        var canvas = e.Surface.Canvas;
        var geo = command.Mesh.Geometry;
        var points = new SKPoint[geo.Vertices.Length];

        for (int i = 0; i < geo.Vertices.Length; i++)
        {
            var vertex = geo.Vertices[i];
            points[i] = new SKPoint(vertex.X, vertex.Y);
        }

        using var paint = CreatePaint(command.Mesh.Material);

        canvas.SetMatrix(command.Mesh.Transform.ToSKMatrix());
        canvas.DrawVertices(SKVertexMode.Triangles, points, null, null, geo.Indices, paint);
    }

    private SKPaint CreatePaint(IMaterial material)
    {
        if (material is ColorMaterialData colorData)
        {
            var color = new SKColor(
               (byte)(colorData.Color.X * 255),
               (byte)(colorData.Color.Y * 255),
               (byte)(colorData.Color.Z * 255),
               (byte)(colorData.Color.W * 255));

            return new SKPaint()
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
        }

        if (material is GradientMaterialData gradientData)
        {
            var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
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
                GradientMaterialData.GradientMaterialType.Horizontal => SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(1, 0), [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Vertical => SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(0, 1), [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Radial => SKShader.CreateRadialGradient(new SKPoint(0.5f, 0.5f), 0.5f, [color1, color2], null, SKShaderTileMode.Clamp),
                GradientMaterialData.GradientMaterialType.Angular => SKShader.CreateSweepGradient(new SKPoint(0.5f, 0.5f), [color1, color2], null),
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
                    colors[i] = ColorLerp(color1, color2, t);
                    positions[i] = t;
                }

                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(1, 0),
                    colors,
                    positions,
                    SKShaderTileMode.Clamp);
            }

            return paint;
        }

        return new SKPaint() { Color = SKColors.White };
    }

    private static SKColor ColorLerp(SKColor color1, SKColor color2, float t)
    {
        return new SKColor(
            (byte)(color1.Red + ((color2.Red - color1.Red) * t)),
            (byte)(color1.Green + ((color2.Green - color1.Green) * t)),
            (byte)(color1.Blue + ((color2.Blue - color1.Blue) * t)),
            (byte)(color1.Alpha + ((color2.Alpha - color1.Alpha) * t)));
    }
}

public static class SKConversions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SKMatrix ToSKMatrix(this Matrix3x2 matrix)
    {
        return new SKMatrix
        {
            ScaleX = matrix.M11,
            SkewX = matrix.M21,
            TransX = matrix.M31,
            SkewY = matrix.M12,
            ScaleY = matrix.M22,
            TransY = matrix.M32,
            Persp0 = 0,
            Persp1 = 0,
            Persp2 = 1
        };
    }
}