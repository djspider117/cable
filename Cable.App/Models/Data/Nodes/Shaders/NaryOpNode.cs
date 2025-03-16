using Cable.Data;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;

namespace Cable.App.Models.Data.Nodes.Shaders;

public abstract class NaryOpNode<T> : NodeData<T> where T : NaryOperation
{
    protected NaryOpNode(string title, CableDataType inType, CableDataType outType) : base(title, inType, outType)
    {
    }

    protected List<IOperand> GetValidOperands(params IOperand[] operands)
    {
        var rv = new List<IOperand>();
        foreach (var item in operands)
        {
            if (item != null)
                rv.Add(item);
        }
        return rv;
    }
}

[NodeData]
[Slot<IOperand>("Input1")]
[Slot<IOperand>("Input2")]
public partial class NaryOp2Node<T> : NaryOpNode<NaryOperation> where T : NaryOperation, new()
{
    public NaryOp2Node(string title) : base(title, CableDataType.None, CableDataType.ShaderInstruction) { }

    public override NaryOperation? GetTypedOutput() => new T() { Operands = GetValidOperands(Input1, Input2) };
}

[NodeData]
[Slot<IOperand>("Input1")]
[Slot<IOperand>("Input2")]
[Slot<IOperand>("Input3")]
public partial class NaryOp3Node<T> : NaryOpNode<NaryOperation> where T : NaryOperation, new()
{
    public NaryOp3Node(string title) : base(title, CableDataType.None, CableDataType.ShaderInstruction) { }

    public override NaryOperation? GetTypedOutput() => new T() { Operands = GetValidOperands(Input1, Input2, Input3) };
}

[NodeData]
[Slot<IOperand>("Input1")]
[Slot<IOperand>("Input2")]
[Slot<IOperand>("Input3")]
[Slot<IOperand>("Input4")]
public partial class NaryOp4Node<T> : NaryOpNode<NaryOperation> where T : NaryOperation, new()
{
    public NaryOp4Node(string title) : base(title, CableDataType.None, CableDataType.ShaderInstruction) { }

    public override NaryOperation? GetTypedOutput() => new T() { Operands = GetValidOperands(Input1, Input2, Input3, Input4) };
}


public partial class Add2Node() : NaryOp2Node<Add>("Add 2");
public partial class Add3Node() : NaryOp3Node<Add>("Add 3");
public partial class Add4Node() : NaryOp4Node<Add>("Add 4");

public partial class Mul2Node() : NaryOp2Node<Mul>("Mul 2");
public partial class Mul3Node() : NaryOp3Node<Mul>("Mul 3");
public partial class Mul4Node() : NaryOp4Node<Mul>("Mul 4");