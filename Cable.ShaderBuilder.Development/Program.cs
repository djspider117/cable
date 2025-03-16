using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Cable.ShaderBuilder.Development;

public class Program
{
    static void Main(string[] args)
    {
        Vec3Value v3_1 = new Vec3Declaration() { VariableName = "q1", Value = new Vector3(0, 2, 4) };
        VectorVariant uv_xyx = new() { Pattern = "xyx", Input = UVValue.Instance };
        AddOperation add_1 = new() { Operands = [TimeValue.Instance, uv_xyx, v3_1] };

        FloatValue floatVal = new() { Value = 0.5f };
        Cos cos = new() { Expression = add_1 };

        MulOperation mul_1 = new() { Operands = [floatVal, cos] };
        AddOperation add_2 = new() { Operands = [floatVal, mul_1] };

        Vec3Value v3_2 = new() { Expression = add_2 };
        Vec3Declaration v3_decl = new() { VariableName = "col", Vec3Reference = v3_2 };

        var builder = new ShaderBuilder
        {
            Uniforms =
            [
                new() { Type = ShaderValueType.Float, Name = "iTime" },
                new() { Type = ShaderValueType.Vector2, Name = "iResolution" },
            ],
            Output = new OutputInstruction() { InputVec3 = v3_decl, OutputAlpha = 0 }
        };

        Console.WriteLine(builder.BuildShader());
    }
}
