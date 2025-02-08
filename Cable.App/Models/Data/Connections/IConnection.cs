using System.ComponentModel;

namespace Cable.App.Models.Data.Connections;

public interface IConnection : INotifyPropertyChanged
{
    string? PropertyName { get; }
}

public interface IConnection<T> : IConnection
{
    T? GetValue();
}


