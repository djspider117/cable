using System.Text;

namespace Cable.ShaderBuilder.Development;

public class ShaderBuilder
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

        sb.AppendLine("vec4 main(vec2 fragCoord)");
        sb.AppendLine("{");
        sb.AppendLine("\tvec2 uv = fragCoord/iResolution.xy;");
        sb.AppendLine($"\t{Output}");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
