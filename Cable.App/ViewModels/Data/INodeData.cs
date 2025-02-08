using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.ViewModels.Data;

public interface INodeData : IDataOutput
{
    string Title { get; }

    CableDataType InputType { get; }
    CableDataType OutputType { get; }

    IEnumerable<IPropertyEditor> GetPropertyEditors();
}

public interface IDataOutput
{
    object? GetOutput();
}
