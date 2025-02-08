using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.ViewModels.Data;

public partial class NodeViewModel : ObservableObject
{
    private readonly NodeDataBase _data;

    [ObservableProperty]
    private double _x = 0d;

    [ObservableProperty]
    private double _y = 0d;

    public string Title => _data.Title;
    public CableDataType InputType => _data.InputType;
    public CableDataType OutputType => _data.OutputType;

    public IEnumerable<IPropertyEditor> PropertyEditors { get; set; }

    public NodeViewModel()
    {
        _data = new EmptyNode();
        PropertyEditors = [];
    }

    public NodeViewModel(NodeDataBase data)
    {
        _data = data;
        PropertyEditors = data.GetPropertyEditors();
    }
}
