using Cable.Data.Types;
using SkiaSharp;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cable.Renderer;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SKPaintStyle ToSKPaintStyle(this MaterialOptions opts)
    {
        if (opts.IsFill)
            return SKPaintStyle.Fill;

        if (opts.IsFill && opts.ApplyBorder)
            return SKPaintStyle.StrokeAndFill;

        if (!opts.IsFill || opts.ApplyBorder)
            return SKPaintStyle.Stroke;

        return SKPaintStyle.Fill;
    }

    public static SKPoint ToSKPoint(this Vector2 point) => new(point.X, point.Y);
}