namespace Cable.ShaderBuilder.Development;

public class OutputInstruction : IOutput
{
    public Vec3Value? InputVec3 { get; set; }
    public Vec4Value? InputVec4 { get; set; }

    public float OutputAlpha { get; set; } = 1;

    public override string ToString()
    {
        if (InputVec4 != null)
        {
            return $"return {InputVec4};";
        }

        if (InputVec3 != null)
        {
            string decl = string.Empty;
            if (InputVec3 is IDeclaration decllarations)
            {
                decl = string.Join("\r\n", decllarations.GetDeclarations());
            }

            var v4 = new Vec4Value { Vec3 = InputVec3, W = OutputAlpha };
            return $"{decl}\r\n\treturn {v4};";
        }

        return "\treturn vec4(1.0,0.0,1.0,1.0);";
    }
}
