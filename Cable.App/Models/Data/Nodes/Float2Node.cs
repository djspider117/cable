using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Vector", CableDataType.None, CableDataType.Float2)]
[Slot<Vector2, Float2Editor>("Value")]
public partial class Float2Node : NodeDataBase
{
    public Float2Node(Vector2 f) : this() => _value = f;

    public override object? GetOutput() => Value;
}
