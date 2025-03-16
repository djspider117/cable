namespace Cable.ShaderBuilder.Development;

public class Cos : ShaderInstructionBase, ITrigFunction
{
    public IExpression Expression { get; set; }

    public override string ToString() => $"cos({Expression})";

    public override bool HasDeclarations => Expression?.HasDeclarations ?? false;

    protected override IEnumerable<IDeclaration> GetDeclarations()
    {
        if (Expression?.HasDeclarations is true)
        {
            foreach (var decl in Expression.Declarations)
            {
                yield return decl;
            }
        }
    }
}
