using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public partial class FloatEditor(INodeData parent, string name, Func<float> getter, Action<float> setter)
    : SingleValueEditor<float>(parent, name, getter, setter)
{
    public override IConnection CreateConnectionAsDestination(INodeData source) => new FloatConnection(source, Parent, DisplayName);
    public override IConnection CreateConnectionAsSource(INodeData destination) => new FloatConnection(Parent, destination, DisplayName);
}
