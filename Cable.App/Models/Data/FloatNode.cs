using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.ViewModels.Data;

[NodeData]
public partial class FloatNode : NodeDataBase
{
    [PropertyEditor<FloatEditor>]
    private float _value;

    public FloatNode()
        : base("Float", CableDataType.None, CableDataType.Float)
    {

    }

    public FloatNode(float f) : this()
    {
        _value = f;
    }

    public override object? GetOutput() => Value;
}