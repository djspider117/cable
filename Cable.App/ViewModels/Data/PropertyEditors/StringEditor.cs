using Cable.App.Models.Data.Connections;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public partial class StringEditor(INodeData parent, string name, Func<string> getter, Action<string> setter)
    : SingleValueEditor<string>(parent, name, getter, setter)
{
    public override IConnection CreateConnectionAsDestination(INodeData source) => new StringConnection(source, Parent, DisplayName);
    public override IConnection CreateConnectionAsSource(INodeData destination) => new StringConnection(Parent, destination, DisplayName);
}