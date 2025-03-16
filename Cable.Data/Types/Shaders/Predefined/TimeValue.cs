namespace Cable.Data.Types.Shaders.Predefined;

public class TimeValue : ShaderInstructionBase, IOperand, IExpression, IVariable
{
    public static readonly TimeValue Instance = new();

    public override string ToString() => "iTime";
}
