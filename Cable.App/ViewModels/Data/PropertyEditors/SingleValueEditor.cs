namespace Cable.App.ViewModels.Data.PropertyEditors;

public abstract class SingleValueEditor<TValue>(INodeData parent, string name, Func<TValue> getter, Action<TValue> setter) 
    : PropertyEditor<TValue>(parent, name, getter, setter)
{
    public TValue Value
    {
        get => GetValueCore()!;
        set => SetProperty(_getter(), value, _setter);
    }

    public override void PushPropertyChanged()
    {
        OnPropertyChanged(nameof(Value));
    }
}
