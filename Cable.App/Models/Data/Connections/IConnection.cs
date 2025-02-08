namespace Cable.App.Models.Data.Connections;

public interface IConnection<T>
{
    T? GetValue();
}


