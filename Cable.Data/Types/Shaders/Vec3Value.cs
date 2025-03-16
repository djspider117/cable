using System.Numerics;

namespace Cable.Data.Types.Shaders;

public class Vec3Value : ShaderInstructionBase, IVariable, IOperand, IExpression
{
    public Vector3 Value { get; set; }
    public IExpression? Expression { get; set; }

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

    public override string ToString()
    {
        if (Expression != null)
            return Expression?.ToString() ?? string.Empty;

        return $"vec3({Value.X}, {Value.Y}, {Value.Z})";
    }
}

public class Vec3Declaration : Vec3Value, IDeclaration
{
    public string VariableName { get; set; }
    public Vec3Value? Vec3Reference { get; set; }

    public override string ToString() => VariableName;
    public override bool HasDeclarations => true;

    protected override IEnumerable<IDeclaration> GetDeclarations()
    {
        if (Vec3Reference != null && Vec3Reference.HasDeclarations is true)
        {
            foreach (var decl in Vec3Reference.Declarations)
            {
                yield return decl;
            }
        }

        if (Expression is not null && Expression.HasDeclarations is true)
        {
            foreach (var decl in Expression.Declarations)
            {
                yield return decl;
            }
        }

        yield return this;
    }

    public IReadOnlyList<string> GetDeclarationTextx()
    {
        if (Vec3Reference != null)
            return [$"vec3 {VariableName} = {Vec3Reference};"];

        if (Expression != null)
            return [$"vec3 {VariableName} = {Expression};"];

        Vec3Reference = new Vec3Value { Value = Value };
        return GetDeclarationTextx();
    }
}
