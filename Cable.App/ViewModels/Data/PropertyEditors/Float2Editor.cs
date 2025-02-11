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
