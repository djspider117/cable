using System.Numerics;

namespace Cable.Data.Types;

public readonly struct Geometry2D(Vector2[] vertices, ushort[] indices) : ICableDataType
{
    public readonly Vector2[] Vertices = vertices;
    public readonly ushort[] Indices = indices;
}