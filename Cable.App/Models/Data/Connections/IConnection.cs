using Cable.App.ViewModels.Data;
using System.ComponentModel;

namespace Cable.App.Models.Data.Connections;

public interface IConnection : INotifyPropertyChanged
{
    string? PropertyName { get; }

    string? SourcePropertyName { get; }

   INodeData SourceNode { get; }
   INodeData TargetNode { get; }
}

public interface IConnection<T> : IConnection
{
    T? GetValue();
}


