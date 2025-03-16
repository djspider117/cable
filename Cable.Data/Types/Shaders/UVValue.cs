namespace Cable.Data.Types.Shaders;

public class UVValue : ShaderInstructionBase, IOperand, IExpression, IVariable
{
    public static readonly UVValue Instance = new();

    public override string ToString() => "uv";
}
