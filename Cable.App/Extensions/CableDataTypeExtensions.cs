using Cable.App.Models.Data;
using System.Numerics;

namespace Cable.App.Extensions;

public static class CableDataTypeExtensions
{
    public static Type? ToPlatformType(this CableDataType type)
    {
        return type switch
        {
            CableDataType.Int => typeof(int),
            CableDataType.Float => typeof(float),
            CableDataType.Float2 => typeof(Vector2),
            _ => null
        };
    }
}