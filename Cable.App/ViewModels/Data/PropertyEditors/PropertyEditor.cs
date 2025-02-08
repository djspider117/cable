namespace Cable.App.ViewModels.Data.PropertyEditors;

public abstract class PropertyEditor<T>(string name, Func<T> getter, Action<T> setter) : ObservableObject, IPropertyEditor
{
    protected readonly Func<T> _getter = getter;
    protected readonly Action<T> _setter = setter;

    public string DisplayName { get; } = name;
}
