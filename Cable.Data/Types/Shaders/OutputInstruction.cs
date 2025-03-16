namespace Cable.Data.Types.Shaders;

public class OutputInstruction : ShaderInstructionBase, IOutput
{
    public Vec3Value? InputVec3 { get; set; }
    public Vec4Value? InputVec4 { get; set; }

    public float OutputAlpha { get; set; } = 1;

    public override string ToString()
    {
        if (InputVec4 != null)
        {
            var decl = string.Empty;
            if (InputVec4.HasDeclarations)
            {
                var d = InputVec4.Declarations.Select(x => string.Join("\r\n", x.Declarations));
                decl = string.Join("\r\n", d);
            }

            return $"{decl}\r\n\treturn {InputVec4};";
        }

        if (InputVec3 != null)
        {
            var decl = string.Empty;
            if (InputVec3.HasDeclarations)
            {
                var d = InputVec3.Declarations.Select(x => string.Join("\r\n", x.GetDeclarationTextx()));
                decl = string.Join("\r\n", d);
            }

            var v4 = new Vec4Value { Vec3 = InputVec3, W = OutputAlpha };
            return $"{decl}\r\nreturn {v4};";
        }

        return "return vec4(1.0,0.0,1.0,1.0);";
    }
}
