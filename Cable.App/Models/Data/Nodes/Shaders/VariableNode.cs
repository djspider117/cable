using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Predefined;

namespace Cable.App.Models.Data.Nodes.Shaders;

[NodeData]
public partial class VariableNode<T> : NodeData<IVariable> where T : IVariable, new()
{
    public VariableNode(string title) 
        : base(title, CableDataType.None, CableDataType.ShaderInstruction)
    {
    }

    public override IVariable? GetTypedOutput() => new T();
}

public partial class UVNode() : VariableNode<UVValue>("UV");
public partial class TimeNode() : VariableNode<TimeValue>("Time");