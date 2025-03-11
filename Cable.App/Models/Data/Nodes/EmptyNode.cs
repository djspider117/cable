using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;

namespace Cable.App.Models.Data.Nodes;

public class EmptyNode : NodeDataBase
{
    public EmptyNode()
        : base("Empty Node", CableDataType.None, CableDataType.None)
    {

    }

    public override IEnumerable<IPropertyEditor> GetPropertyEditors()
    {
        return [];
    }

    public override object? GetOutput() => null;
}