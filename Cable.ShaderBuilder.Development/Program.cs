using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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

        var builder = new ShaderBuilder
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

public class VariableNameGenerator
{
    private static readonly HashSet<string> _keywords = new()
    {
        "attribute", "const", "uniform", "varying", "layout", "centroid", "flat", "smooth", "noperspective", "patch",
        "sample", "break", "continue", "do", "for", "while", "switch", "case", "default", "if", "else", "subroutine",
        "in", "out", "inout", "int", "void", "bool", "true", "false", "float", "double", "discard", "return", "mat2",
        "mat3", "mat4", "dmat2", "dmat3", "dmat4", "mat2x2", "mat2x3", "mat2x4", "mat3x2", "mat3x3", "mat3x4",
        "mat4x2", "mat4x3", "mat4x4", "dmat2x2", "dmat2x3", "dmat2x4", "dmat3x2", "dmat3x3", "dmat3x4", "dmat4x2",
        "dmat4x3", "dmat4x4", "lowp", "mediump", "highp", "precision", "sampler1D", "sampler2D", "sampler3D",
        "samplerCube", "sampler1DShadow", "sampler2DShadow", "sampler1DArray", "sampler2DArray", "sampler1DArrayShadow",
        "sampler2DArrayShadow", "sampler2DMS", "sampler2DMSArray", "samplerCubeShadow", "samplerBuffer", "sampler2DRect",
        "sampler2DRectShadow", "isampler1D", "isampler2D", "isampler3D", "isamplerCube", "isampler1DArray", "isampler2DArray",
        "isampler2DMS", "isampler2DMSArray", "isamplerBuffer", "isampler2DRect", "usampler1D", "usampler2D", "usampler3D",
        "usamplerCube", "usampler1DArray", "usampler2DArray", "usampler2DMS", "usampler2DMSArray", "usamplerBuffer",
        "usampler2DRect", "image1D", "image2D", "image3D", "imageCube", "iimage1D", "iimage2D", "iimage3D", "iimageCube",
        "uimage1D", "uimage2D", "uimage3D", "uimageCube", "image1DArray", "image2DArray", "iimage1DArray", "iimage2DArray",
        "uimage1DArray", "uimage2DArray", "image2DMS", "iimage2DMS", "uimage2DMS", "image2DMSArray", "iimage2DMSArray",
        "uimage2DMSArray", "struct", "uint", "uvec2", "uvec3", "uvec4", "vec2", "vec3", "vec4", "void", "volatile", "uv", "fragCoord"
    };

    private HashSet<string> _existingVariableNames = [];
    private Random _random = new();
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";

    public string CreateVariable()
    {
        while (true)
        {
            string name = GenerateRandomVariableName();
            if (!_existingVariableNames.Contains(name) && !_keywords.Contains(name))
            {
                _existingVariableNames.Add(name);
                return name;
            }
        }
    }

    private string GenerateRandomVariableName()
    {
        int length = _random.Next(3, 10);
        StringBuilder sb = new();

        sb.Append(Alphabet[_random.Next(Alphabet.Length)]);

        for (int i = 1; i < length; i++)
        {
            if (_random.Next(2) == 0)
                sb.Append(Alphabet[_random.Next(Alphabet.Length)]);
            else
                sb.Append(Digits[_random.Next(Digits.Length)]);
        }

        return sb.ToString();
    }
}
