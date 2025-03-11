using System.Numerics;

namespace Cable.Data.Types;

public readonly record struct ColorMaterialData(Vector4 Color) : IMaterial;
