using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Renderable", CableDataType.None, CableDataType.Renderable)]
[Slot<IShape>("Shape")]
[Slot<Transform>("Transform")]
[Slot<IMaterial>("Material")]
public partial class RenderableNode : NodeData<RenderableElement>
{
    public override RenderableElement GetTypedOutput()
    {
        return new RenderableElement(Shape, Material, Transform);
    }
}