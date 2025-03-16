using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;
using Cable.Data.Types.Shaders.Predefined;
using Cable.Data.Types.Shaders.Special;
using System.Numerics;

namespace Cable;

public class Program
{
    static void Main(string[] args)
    {
        var nameGen = new VariableNameGenerator();

        Vec3Value v3_1 = new() { Value = new Vector3(0, 2, 4) };
        VectorVariant uv_xyx = new() { Pattern = "xyx", Input = UVValue.Instance };
        Add add_1 = new() { Operands = [TimeValue.Instance, uv_xyx, v3_1] };

        FloatValue floatVal = new() { Value = 0.5f };
        Cos cos = new() { Expression = add_1 };

        Mul mul_1 = new() { Operands = [floatVal, cos] };
        Add add_2 = new() { Operands = [floatVal, mul_1] };

        Vec3Value v3_2 = new() { Expression = add_2 };
        //Vec3Declaration v3_decl = new() { VariableName = nameGen.CreateVariable(), Expression = add_2 };

        var builder = new ShaderBuilder
        {
            Uniforms =
            [
                new() { Type = ShaderValueType.Float, Name = "iTime" },
                new() { Type = ShaderValueType.Vector2, Name = "iResolution" },
            ],
            Output = new OutputInstruction() { InputVec3 = v3_2, OutputAlpha = 0 }
        };

        var shaderText = builder.BuildShader();
        Console.WriteLine(shaderText);
    }
}
