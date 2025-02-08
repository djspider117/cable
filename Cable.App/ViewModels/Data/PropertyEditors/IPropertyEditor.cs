using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public interface IPropertyEditor
{
    INodeData Parent { get; set; }

    string DisplayName { get; }
    void PushPropertyChanged();

    IConnection CreateConnectionAsDestination(INodeData source);
    IConnection CreateConnectionAsSource(INodeData destination);
}
