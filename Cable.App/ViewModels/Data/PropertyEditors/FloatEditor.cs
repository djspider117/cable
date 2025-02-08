using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class FloatEditor(INodeData parent, string name, Func<float> getter, Action<float> setter)
    : PropertyEditor<float>(parent, name, getter, setter)
{
    public float Value
    {
        get => GetValueCore();
        set => SetProperty(_getter(), value, _setter);
    }
    public override void PushPropertyChanged()
    {
        OnPropertyChanged(nameof(Value));
    }

    public override IConnection CreateConnectionAsDestination(INodeData source)
    {
        return new FloatConnection(source, Parent, DisplayName);
    }

    public override IConnection CreateConnectionAsSource(INodeData destination)
    {
        return new FloatConnection(Parent, destination, DisplayName);
    }
}
