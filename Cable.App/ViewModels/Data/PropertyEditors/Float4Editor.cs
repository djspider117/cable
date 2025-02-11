using Cable.App.Models.Data.Connections;
using System.Numerics;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class Float4Editor(INodeData parent, string name, Func<Vector4> getter, Action<Vector4> setter)
    : PropertyEditor<Vector4>(parent, name, getter, setter)
{
    public float ValueX
    {
        get => GetValueCore().X;
        set => SetProperty(_getter(), new Vector4(value, ValueY, ValueZ, ValueW), _setter);
    }

    public float ValueY
    {
        get => GetValueCore().Y;
        set => SetProperty(_getter(), new Vector4(ValueX, value, ValueZ, ValueW), _setter);
    }
    public float ValueZ
    {
        get => GetValueCore().Z;
        set => SetProperty(_getter(), new Vector4(ValueX, ValueY, value, ValueW), _setter);
    }

    public float ValueW
    {
        get => GetValueCore().W;
        set => SetProperty(_getter(), new Vector4(ValueX, ValueY, ValueZ, value), _setter);
    }


    public override void PushPropertyChanged()
    {
        OnPropertyChanged(nameof(ValueX));
        OnPropertyChanged(nameof(ValueY));
        OnPropertyChanged(nameof(ValueZ));
        OnPropertyChanged(nameof(ValueW));
    }

    public override IConnection CreateConnectionAsDestination(INodeData source)
    {
        return new Float4Connection(source, Parent, DisplayName);
    }

    public override IConnection CreateConnectionAsSource(INodeData destination)
    {
        return new Float4Connection(Parent, destination, DisplayName);
    }
}