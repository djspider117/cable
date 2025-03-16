namespace Cable.Data.Types.Shaders;

public class TimeValue : ShaderInstructionBase, IOperand, IExpression, IVariable
{
    public static readonly TimeValue Instance = new();

    public override string ToString() => "iTime";
}
