using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.ComponentModel;

namespace Cable.App.ViewModels.Data;

public interface INodeData : IDataOutput, INotifyPropertyChanged
{
    string Title { get; }

    CableDataType InputType { get; }
    CableDataType OutputType { get; }

    IEnumerable<IPropertyEditor> GetPropertyEditors();
    RenderCommandList GetRenderCommands();
}

public interface IDataOutput
{
    object? GetOutput();
}
