using System.Numerics;

namespace Cable.ShaderBuilder.Development;

public class Vec3Value : IVariable, IOperand, IExpression
{
    public Vector3 Value { get; set; }
    public IExpression? Expression { get; set; }

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

    public IReadOnlyList<string> GetDeclarations()
    {
        if (Vec3Reference != null)
            return [$"vec3 {VariableName} = {Vec3Reference};"];

        Vec3Reference = new Vec3Value { Value = Value };
        return GetDeclarations();
    }
}
