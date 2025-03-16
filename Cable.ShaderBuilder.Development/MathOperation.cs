
namespace Cable.ShaderBuilder.Development;

public abstract class MathOperation : ShaderInstructionBase, IExpression, IOperand
{
    public List<IOperand> Operands { get; set; } = [];
    public abstract string Operator { get; }

    public override bool HasDeclarations => Operands.Any(x => x.HasDeclarations);

    protected override IEnumerable<IDeclaration> GetDeclarations()
    {
        return Operands.Where(x => x.HasDeclarations).SelectMany(x => x.Declarations).Cast<IDeclaration>();
    }

    public override string ToString()
    {
        if (Operands.Count == 0)
            return "0";

        if (Operands.Count == 1)
            return Operands[0]?.ToString() ?? "0";

        return string.Join($" {Operator} ", Operands.Select(x =>
        {
            if (x is MathOperation)
                return $"({x})";

            return x.ToString();
        }));
    }
}

public class AddOperation : MathOperation
{
    public override string Operator => "+";
}

public class MulOperation : MathOperation
{
    public override string Operator => "*";
}
