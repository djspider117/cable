using System.Text;

namespace Cable.Data.Types.Shaders;

public class ShaderBuilder : ICableDataType
{
    public List<UniformDefinition> Uniforms { get; set; } = [];

    public IOutput Output { get; set; }

    public string BuildShader()
    {
        var sb = new StringBuilder();

        foreach (var uniform in Uniforms)
        {
            sb.AppendLine(uniform.ToString());
        }
        sb.AppendLine();

        sb.AppendLine("vec4 main(vec2 fragCoord){");
        sb.AppendLine();
        sb.AppendLine("vec2 uv = fragCoord/iResolution.xy;");
        sb.AppendLine($"{Output}");
        sb.AppendLine();
        sb.AppendLine("}");

        return sb.ToString();
    }
}

public readonly record struct FileData(string Path) : ICableDataType;