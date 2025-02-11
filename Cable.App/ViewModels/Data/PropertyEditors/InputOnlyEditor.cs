using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public partial class InputOnlyEditor(INodeData parent, string name) : ObservableObject, IPropertyEditor
{
    public INodeData Parent { get; set; } = parent;
    public string DisplayName { get; } = name;

    [ObservableProperty]
    private bool _isConnected;

    public void PushPropertyChanged()
    {
        throw new NotImplementedException();
    }

    public IConnection CreateConnectionAsDestination(INodeData source)
    {
        throw new NotImplementedException();
    }

    public IConnection CreateConnectionAsSource(INodeData destination)
    {
        throw new NotImplementedException();
    }
}