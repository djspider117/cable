using System.Numerics;

namespace Cable.Data.Types;

public readonly struct Camera2D(float zoom, Transform transform) : ICableDataType
{
    public readonly float Zoom = zoom;
    public readonly Transform Transform = transform;
}
