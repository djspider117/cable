namespace Cable.Data.Types.Shaders.Math;

public class ZeroExpression : IExpression
{
    public static readonly ZeroExpression Instance = new();

    public bool HasDeclarations => false;
    public IEnumerable<IDeclaration> Declarations => [];

    public override string ToString() => "0";
}