using System.Numerics;

namespace Cable.Data.Types;

public readonly struct GradientMaterialData(
    Vector4 color1,
    Vector4 color2, 
    GradientMaterialData.GradientMaterialType type, 
    GradientMaterialData.GradientRenderMode 
    renderMode, 
    short steps,
    MaterialOptions materialOptions = default)
    : IMaterial
{
    public readonly Vector4 Color1 = color1;
    public readonly Vector4 Color2 = color2;
    public readonly GradientMaterialType Type = type;
    public readonly GradientRenderMode RenderMode = renderMode;
    public readonly short Steps = steps;
    public readonly MaterialOptions MaterialOptions = materialOptions;

    public GradientMaterialData(Vector4 color1, Vector4 color2) : this(color1, color2, GradientMaterialType.Vertical, GradientRenderMode.Smooth, 8)
    {
    }

    public enum GradientMaterialType : byte
    {
        Horizontal,
        Vertical,
        Radial,
        Angular
    }

    public enum GradientRenderMode : byte
    {
        Smooth,
        Stepped
    }
}
