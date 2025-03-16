namespace Cable.Data.Types.Shaders;

public class FloatValue : ShaderInstructionBase, IOperand, IExpression
{
    public float Value { get; set; }

    public override string ToString() => Value.ToString();
}
