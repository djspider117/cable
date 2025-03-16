namespace Cable.Data.Types.Shaders;

public class VectorVariant : ShaderInstructionBase, IOperand, IVariable
{
    public string Pattern { get; set; } = "xyz";
    public IVariable Input { get; set; }

    public override string ToString() => $"{Input}.{Pattern}";
}
