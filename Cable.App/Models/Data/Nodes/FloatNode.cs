using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Float", CableDataType.None, CableDataType.Float2)]
[Slot<float, FloatEditor>("Value")]
public partial class FloatNode : NodeDataBase
{
    public FloatNode(float f) : this() => _value = f;

    public override object? GetOutput() => Value;
}