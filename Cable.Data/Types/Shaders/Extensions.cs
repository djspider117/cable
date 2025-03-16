namespace Cable.Data.Types.Shaders;

public static class Extensions
{
    public static string ToShaderString(this ShaderValueType builder)
    {
        return builder switch
        {
            ShaderValueType.Float => "float",
            ShaderValueType.Vector2 => "vec2",
            ShaderValueType.Vector3 => "vec3",
            ShaderValueType.Vector4 => "vec4",
            _ => throw new InvalidOperationException("Unknown GlSL type"),
        };
    }
}