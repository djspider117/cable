﻿namespace Cable.ShaderBuilder.Development;

public class VectorVariant : ShaderInstructionBase, IOperand, IVariable
{
    public string Pattern { get; set; } = "xyz";
    public IVariable Input { get; set; }

    public override string ToString() => $"{Input}.{Pattern}";
}
