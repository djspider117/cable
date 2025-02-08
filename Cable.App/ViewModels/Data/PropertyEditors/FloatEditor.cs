namespace Cable.App.ViewModels.Data.PropertyEditors;

public class FloatEditor(string name, Func<float> getter, Action<float> setter)
    : PropertyEditor<float>(name, getter, setter)
{
    public float Value
    {
        get => _getter();
        set => SetProperty(_getter(), value, _setter);
    }
}
