namespace Cable.ShaderBuilder.Development;

public class Vec4Value : IVariable, IOperand
{
    public Vec3Value? Vec3 { get; set; }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public override string ToString()
    {
        if (Vec3 != null)
            return $"vec4({Vec3}, {W})";

        return $"vec4({X}, {Y}, {Z}, {W})";
    }
}
