using Cable.Data;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;

namespace Cable.App.Models.Data.Nodes.Shaders;

[NodeData]
[Slot<IExpression>("Expression")]
public partial class TrigNode<T> : NodeData<TrigonometricFunction> where T : TrigonometricFunction, new()
{
    public TrigNode(string title) : base(title, CableDataType.None, CableDataType.ShaderInstruction) { }

    public override TrigonometricFunction? GetTypedOutput() => new T() { Expression = Expression };
}

public partial class SinNode() : TrigNode<Sin>("Sin");
public partial class CosNode() : TrigNode<Cos>("Cos");
public partial class TanNode() : TrigNode<Tan>("Tan");

public partial class ASinNode() : TrigNode<ASin>("ASin");
public partial class ACosNode() : TrigNode<ACos>("ACos");
public partial class ATanNode() : TrigNode<ATan>("ATan");

public partial class RadiansNode() : TrigNode<Radians>("Radians");
public partial class DegreesNode() : TrigNode<Degrees>("Degrees");