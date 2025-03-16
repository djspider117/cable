namespace Cable.ShaderBuilder.Development;

public enum ShaderValueType
{
    Float,
    Vector2,
    Vector3,
    Vector4
}

public class UniformDefinition : IShaderInstruction
{
    public ShaderValueType Type { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"uniform {Type.ToShaderString()} {Name};";
    }
}
