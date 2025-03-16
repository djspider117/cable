namespace Cable.Data.Types.Shaders.Math;

public abstract class TrigonometricFunction : ShaderInstructionBase, ITrigFunction
{
    public string FunctionName { get; init; }

    public IExpression? Expression { get; set; }

    protected TrigonometricFunction(string fnName)
    {
        FunctionName = fnName;
    }

    public override string ToString() => $"{FunctionName}({Expression ?? ZeroExpression.Instance})";

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

public class Sin() : TrigonometricFunction("sin");
public class Cos() : TrigonometricFunction("cos");
public class Tan() : TrigonometricFunction("tan");

public class ASin() : TrigonometricFunction("asin");
public class ACos() : TrigonometricFunction("acos");
public class ATan() : TrigonometricFunction("atan");

public class Radians() : TrigonometricFunction("radians");
public class Degrees() : TrigonometricFunction("degrees");
