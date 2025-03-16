namespace Cable.Data.Types.Shaders.Math;

public abstract class NaryOperation : ShaderInstructionBase, IExpression, IOperand
{
    public List<IOperand> Operands { get; set; } = [];
    public string Operator { get; init; }

    public override bool HasDeclarations => Operands.Any(x => x.HasDeclarations);

    protected NaryOperation(string op) => Operator = op;

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
            if (x is NaryOperation)
                return $"({x})";

            return x.ToString();
        }));
    }
}

public class Add() : NaryOperation("+");
public class Mul() : NaryOperation("*");
public class Sub() : NaryOperation("-");
public class Div() : NaryOperation("/");
public class Mod() : NaryOperation("%");