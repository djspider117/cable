using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;
using Cable.Data.Types.Shaders.Special;
using Cable.Data.Types.Shaders.Trig;
using System.Numerics;

namespace Cable.ShaderBuilder.Development;

public class Program
{
    static void Main(string[] args)
    {
        var nameGen = new VariableNameGenerator();

        Vec3Value v3_1 = new Vec3Declaration() { VariableName = nameGen.CreateVariable(), Value = new Vector3(0, 2, 4) };
        VectorVariant uv_xyx = new() { Pattern = "xyx", Input = UVValue.Instance };
        AddOperation add_1 = new() { Operands = [TimeValue.Instance, uv_xyx, v3_1] };

        Vec3Declaration add_declr = new() { VariableName = nameGen.CreateVariable(), Expression = add_1 };

        FloatValue floatVal = new() { Value = 0.5f };
        Cos cos = new() { Expression = add_declr };

        MulOperation mul_1 = new() { Operands = [floatVal, cos] };
        Vec3Declaration mul_declr = new() { VariableName = nameGen.CreateVariable(), Expression = mul_1 };

        AddOperation add_2 = new() { Operands = [floatVal, mul_declr] };

        Vec3Value v3_2 = new() { Expression = add_2 };
        Vec3Declaration v3_decl = new() { VariableName = nameGen.CreateVariable(), Vec3Reference = v3_2 };

        var builder = new Cable.Data.Types.Shaders.ShaderBuilder
        {
            Uniforms =
            [
                new() { Type = ShaderValueType.Float, Name = "iTime" },
                new() { Type = ShaderValueType.Vector2, Name = "iResolution" },
            ],
            Output = new OutputInstruction() { InputVec3 = v3_decl, OutputAlpha = 0 }
        };

        var shaderText = builder.BuildShader();
        Console.WriteLine(shaderText);
    }
}
