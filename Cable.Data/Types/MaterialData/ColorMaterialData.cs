using System.Numerics;

namespace Cable.Data.Types.MaterialData;

public readonly record struct ColorMaterialData(Vector4 Color, MaterialOptions MaterialOptions = default) : IMaterial
{
    public static readonly ColorMaterialData InvalidColor = new(new Vector4(1,0,1,1));
}
