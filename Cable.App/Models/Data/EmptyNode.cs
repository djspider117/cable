using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.Models.Data;

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