using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public abstract partial class PropertyEditor<T>(INodeData parent, string name, Func<T> getter, Action<T> setter) : ObservableObject, IPropertyEditor
{
    protected readonly Func<T> _getter = getter;
    protected readonly Action<T> _setter = setter;

    [ObservableProperty]
    private bool _isConnected;

    public Func<object>? DataGetter { get; set; }

    public string DisplayName { get; } = name;
    public INodeData Parent { get; set; } = parent;

    public override int GetHashCode() => DisplayName.GetHashCode();
    public override bool Equals(object? obj) => GetHashCode() == obj?.GetHashCode();

    protected T? GetValueCore()
    {
        if (IsConnected)
        {
            var data = DataGetter?.Invoke();
            if (data == null)
                return default;

            return (T)data;
        }

        return _getter();
    }

    public abstract void PushPropertyChanged();

    public abstract IConnection CreateConnectionAsDestination(INodeData source);
    public abstract IConnection CreateConnectionAsSource(INodeData destination);
}