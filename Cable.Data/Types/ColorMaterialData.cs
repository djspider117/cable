using System.Numerics;

namespace Cable.Data.Types;

public readonly record struct ColorMaterialData(Vector4 Color, MaterialOptions MaterialOptions = default) : IMaterial;

public readonly record struct ScrollingColorsMaterialData() : IMaterial;

public readonly record struct NoiseMaterialData : IMaterial
{

}