using Cable.Data.Extensions;

namespace Cable.Data.Types.Shaders.Special;

public enum ShaderValueType
{
    Float,
    Vector2,
    Vector3,
    Vector4
}

public class UniformDefinition : ShaderInstructionBase, IShaderInstruction
{
    public ShaderValueType Type { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"uniform {Type.ToShaderString()} {Name};";
    }
}
