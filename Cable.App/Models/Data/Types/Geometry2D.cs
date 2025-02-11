using System.Numerics;

namespace Cable.App.Models.Data.Types;

public interface ICableDataType { }
public interface IMaterial : ICableDataType { }

public readonly struct Geometry2D(Vector2[] vertices, int[] indices) : ICableDataType
{
    public readonly Vector2[] Vertices = vertices;
    public readonly int[] Indices = indices;
}

public readonly record struct ColorMaterialData(Vector4 Color) : IMaterial;

public readonly struct GradientMaterialData(Vector4 color1, Vector4 color2, GradientMaterialData.GradientMaterialType type, GradientMaterialData.GradientRenderMode renderMode, short steps) : IMaterial
{
    public readonly Vector4 Color1 = color1;
    public readonly Vector4 Color2 = color2;
    public readonly GradientMaterialType Type = type;
    public readonly GradientRenderMode RenderMode = renderMode;
    public readonly short Steps = steps;

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

public readonly struct Mesh2D(Geometry2D geometry, Matrix3x2 transform, IMaterial material) : ICableDataType
{
    public readonly Geometry2D Geometry = geometry;
    public readonly Matrix3x2 Transform = transform;
    public readonly IMaterial Material = material;
}

public readonly struct Camera2D(float zoom, Matrix3x2 transform) : ICableDataType
{
    public readonly float Material = zoom;
    public readonly Matrix3x2 Transform = transform;
}

public readonly struct TextureRGBA(float width, float height, byte[] data) : ICableDataType
{
    public readonly float Width = width;
    public readonly float Height = height;
    public readonly byte[] Data = data;
}