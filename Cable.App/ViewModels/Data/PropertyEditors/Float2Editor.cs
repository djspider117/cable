using System.Numerics;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class Float2Editor(string name, Func<Vector2> getter, Action<Vector2> setter)
    : PropertyEditor<Vector2>(name, getter, setter)
{
    public float ValueX
    {
        get => _getter().X;
        set => SetProperty(_getter(), new Vector2(value, ValueY), _setter);
    }

    public float ValueY
    {
        get => _getter().Y;
        set => SetProperty(_getter(), new Vector2(ValueX, value), _setter);
    }
}
