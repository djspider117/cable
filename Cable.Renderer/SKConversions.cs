﻿using SkiaSharp;
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
}