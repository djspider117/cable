using Cable.Data.Types.MaterialData;
using System.Numerics;

namespace Cable.Data.Types;

public readonly struct Mesh2D(Geometry2D geometry, Matrix3x2 transform, IMaterial material) : ICableDataType
{
    public readonly Geometry2D Geometry = geometry;
    public readonly Matrix3x2 Transform = transform;
    public readonly IMaterial Material = material;
}
