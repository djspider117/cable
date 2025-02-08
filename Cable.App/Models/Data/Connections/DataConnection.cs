using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data;

public abstract partial class DataConnection<T>(INodeData source, INodeData target) : ObservableObject, IConnection<T>
{
    [ObservableProperty]
    private INodeData _sourceNode = source;

    [ObservableProperty]
    private INodeData _targetNode = target;

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


