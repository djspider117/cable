using Cable.App.Models.Data.Connections;
using System.ComponentModel;
using System.Numerics;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class Float2Editor(INodeData parent, string name, Func<Vector2> getter, Action<Vector2> setter)
    : PropertyEditor<Vector2>(parent, name, getter, setter)
{
    public float ValueX
    {
        get => GetValueCore().X;
        set => SetProperty(_getter(), new Vector2(value, ValueY), _setter);
    }

    public float ValueY
    {
        get => GetValueCore().Y;
        set => SetProperty(_getter(), new Vector2(ValueX, value), _setter);
    }

    public override void PushPropertyChanged()
    {
        OnPropertyChanged(nameof(ValueX));
        OnPropertyChanged(nameof(ValueY));
    }

    public override IConnection CreateConnectionAsDestination(INodeData source)
    {
        return new Float2Connection(source, Parent, DisplayName);
    }

    public override IConnection CreateConnectionAsSource(INodeData destination)
    {
        return new Float2Connection(Parent, destination, DisplayName);
    }
}
