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

    [ObservableProperty]
    private string? _sourcePropertyName;

    public DataConnection(INodeData source, INodeData target, string? targetPropName = null, string? sourcePropName = null)
    {
        _sourceNode = source;
        _targetNode = target;
        _propertyName = targetPropName;
        _sourcePropertyName = sourcePropName;

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

        var output = SourcePropertyName != null ? SourceNode.GetPropertyOutput(SourcePropertyName) : SourceNode.GetOutput();
        return output == null ? default : ConvertValue(output);
    }

    public virtual T ConvertValue(object value)
    {
        return (T)value;
    }
}


