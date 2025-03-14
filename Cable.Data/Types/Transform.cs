using System.Numerics;

namespace Cable.Data.Types;

public readonly record struct Transform(Vector2 Translate, float Rotation, Vector2 Scale, Vector2 OriginOffset) : ICableDataType
{
    public static readonly Transform Identity = new(Vector2.Zero, 0, Vector2.One, Vector2.Zero);
}

