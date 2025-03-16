using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.ComponentModel;

namespace Cable.App.ViewModels.Data;

public interface INodeData : IDataOutput, INotifyPropertyChanged
{
    long Id { get; set; }
    string Title { get; }

    double X {get; set; }
    double Y {get; set; }

    CableDataType InputType { get; }
    CableDataType OutputType { get; }

    IDataOutput? IncomingData { get; set; }

    IEnumerable<IPropertyEditor> GetPropertyEditors();
    RasterizerData GetRenderCommands();
}

public interface IDataOutput
{
    object? GetOutput();
    object? GetPropertyOutput(string propertyName);
}
