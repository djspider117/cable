using System.Numerics;

namespace Cable.Data.Types;

public readonly struct Geometry2D(Vector2[] vertices, int[] indices) : ICableDataType
{
    public readonly Vector2[] Vertices = vertices;
    public readonly int[] Indices = indices;
}