using Cable.App.Models.Data.Connections;
using System.ComponentModel;

namespace Cable.App.ViewModels.Data;

public abstract partial class DataConnection<T> : ObservableObject, IConnection<T>
{
    [ObservableProperty]
    private INodeData _sourceNode;

    [ObservableProperty]
    private INodeData _targetNode;

    [ObservableProperty]
    private string? _propertyName;

    public DataConnection(INodeData source, INodeData target, string? propertyName = null)
    {
        _sourceNode = source;
        _targetNode = target;
        _propertyName = propertyName;

        _sourceNode.PropertyChanged += Node_PropertyChanged;
    }

    private void Node_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged("SourceNodeData");
    }

    public T? GetValue()
    {
        if (SourceNode == null)
            return default;

        var output = SourceNode.GetOutput();
        if (output == null)
            return default;

        return (T)output;
    }
}


