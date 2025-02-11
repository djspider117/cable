using Cable.App.Models.Data.Connections;
using System.Numerics;

namespace Cable.App.ViewModels.Data.PropertyEditors;

public class ColorEditor(INodeData parent, string name, Func<Vector4> getter, Action<Vector4> setter)
    : SingleValueEditor<Vector4>(parent, name, getter, setter)
{
    public override IConnection CreateConnectionAsDestination(INodeData source) => new Float4Connection(source, Parent, DisplayName);
    public override IConnection CreateConnectionAsSource(INodeData destination) => new Float4Connection(Parent, destination, DisplayName);
}
