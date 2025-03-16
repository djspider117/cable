using Cable.App.Models.Data.Connections;
using System.Numerics;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class Float3Editor(INodeData parent, string name, Func<Vector3> getter, Action<Vector3> setter)
    : PropertyEditor<Vector3>(parent, name, getter, setter)
{
    public float ValueX
    {
        get => GetValueCore().X;
        set => SetProperty(_getter(), new Vector3(value, ValueY, ValueZ), _setter);
    }

    public float ValueY
    {
        get => GetValueCore().Y;
        set => SetProperty(_getter(), new Vector3(ValueX, value, ValueZ), _setter);
    }
    public float ValueZ
    {
        get => GetValueCore().Z;
        set => SetProperty(_getter(), new Vector3(ValueX, ValueY, value), _setter);
    }

    public override void PushPropertyChanged()
    {
        OnPropertyChanged(nameof(ValueX));
        OnPropertyChanged(nameof(ValueY));
        OnPropertyChanged(nameof(ValueZ));
    }

    public override IConnection CreateConnectionAsDestination(INodeData source)
    {
        return new Float3Connection(source, Parent, DisplayName);
    }

    public override IConnection CreateConnectionAsSource(INodeData destination)
    {
        return new Float3Connection(Parent, destination, DisplayName);
    }
}
