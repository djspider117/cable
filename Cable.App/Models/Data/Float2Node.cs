using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData]
public partial class Float2Node : NodeDataBase
{
    [PropertyEditor<Float2Editor>]
    private Vector2 _value;

    public Float2Node()
        : base("Vector", CableDataType.None, CableDataType.Float2)
    {

    }
    public Float2Node(Vector2 f) : this()
    {
        _value = f;
    }


    public override object? GetOutput() => Value;
}