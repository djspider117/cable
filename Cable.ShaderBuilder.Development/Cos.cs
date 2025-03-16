namespace Cable.ShaderBuilder.Development;

public class Cos : ITrigFunction
{
    public IExpression Expression { get; set; }

    public override string ToString() => $"cos({Expression})";
}
