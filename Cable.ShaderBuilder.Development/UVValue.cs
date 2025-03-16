namespace Cable.ShaderBuilder.Development;

public class UVValue : ShaderInstructionBase, IOperand, IExpression, IVariable
{
    public static readonly UVValue Instance = new();

    public override string ToString() => "uv";
}
