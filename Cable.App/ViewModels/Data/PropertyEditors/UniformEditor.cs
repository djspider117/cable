using Cable.App.Models.Data.Connections;
using Cable.Data.Types.MaterialData;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public partial class UniformEditor(INodeData parent, string name, Func<Uniform> getter, Action<Uniform> setter)
    : SingleValueEditor<Uniform>(parent, name, getter, setter)
{
    public override IConnection CreateConnectionAsDestination(INodeData source) => new UniformCollectionConnection(source, Parent, DisplayName);

    public override IConnection CreateConnectionAsSource(INodeData destination) => new UniformCollectionConnection(Parent, destination, DisplayName);
}