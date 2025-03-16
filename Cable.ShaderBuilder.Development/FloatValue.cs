namespace Cable.ShaderBuilder.Development;

public class FloatValue : IOperand, IExpression
{
    public float Value { get; set; }

    public override string ToString() => Value.ToString();
}
