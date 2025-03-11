using System.Numerics;

namespace Cable.Data.Types;

public readonly struct Camera2D(float zoom, Matrix3x2 transform) : ICableDataType
{
    public readonly float Material = zoom;
    public readonly Matrix3x2 Transform = transform;
}
