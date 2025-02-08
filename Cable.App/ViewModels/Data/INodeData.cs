using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.ComponentModel;

namespace Cable.App.ViewModels.Data;

public interface INodeData : IDataOutput, INotifyPropertyChanged
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
