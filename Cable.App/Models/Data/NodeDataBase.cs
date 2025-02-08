using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.Models.Data;

public abstract class NodeDataBase(string title, CableDataType inType, CableDataType outType) : INodeData
{
    public string Title { get; } = title;
    public CableDataType InputType { get; } = inType;
    public CableDataType OutputType { get; } = outType;

    public IDataOutput? IncomingData { get; }

    public abstract IEnumerable<IPropertyEditor> GetPropertyEditors();

    public abstract object? GetOutput();
}