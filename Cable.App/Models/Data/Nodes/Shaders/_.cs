using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;
using Cable.Data.Types.Shaders.Predefined;
using Cable.Data.Types.Shaders.Special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cable.App.Models.Data.Nodes.Shaders;

public abstract class DeclarationNode<T> : NodeData<T> where T : ICableDataType
{
    protected static readonly VariableNameGenerator _nameGen = App.GetService<VariableNameGenerator>()!;
    protected string _variableName = _nameGen.CreateVariable();

    protected DeclarationNode(string title, CableDataType inType, CableDataType outType) : base(title, inType, outType)
    {
    }
}

[NodeData("Vec3Decl", CableDataType.None, CableDataType.ShaderInstruction)]
[Slot<IExpression>("Expression")]
public partial class ShaderVec3DeclNode : DeclarationNode<Vec3Declaration>
{
    public override Vec3Declaration? GetTypedOutput()
    {
        return new Vec3Declaration { VariableName = _variableName, Expression = Expression };
    }
}

[NodeData("Vec3", CableDataType.None, CableDataType.ShaderInstruction)]
[Slot<Vector3, Float3Editor>("Value")]
[Slot<IExpression>("Expression")]
public partial class ShaderVec3Node : NodeData<Vec3Value>
{
    public override Vec3Value? GetTypedOutput()
    {
        if (Expression != null)
            return new Vec3Value { Expression = Expression };

        return new Vec3Value { Value = Value };
    }
}

[NodeData("Float (shader)", CableDataType.None, CableDataType.ShaderInstruction)]
[Slot<float, FloatEditor>("Value")]
public partial class ShaderFloatNode : NodeData<FloatValue>
{
    public override FloatValue? GetTypedOutput()
    {
        return new FloatValue { Value = Value };
    }
}

[NodeData("Shader Output", CableDataType.None, CableDataType.ShaderInstruction)]
[Slot<Vec3Value>("InputVec3")]
[Slot<Vec4Value>("InputVec4")]
[Slot<float, FloatEditor>("OutputAlpha")]
public partial class OutputInstructionNode : NodeData<OutputInstruction>
{
    public override OutputInstruction? GetTypedOutput()
    {
        if (InputVec3 != null)
            return new OutputInstruction() { InputVec3 = InputVec3, OutputAlpha = OutputAlpha };

        if (InputVec4 != null)
            return new OutputInstruction() { InputVec4 = InputVec4 };

        return new OutputInstruction();
    }
}