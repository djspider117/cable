using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;

namespace Cable.App.ViewModels.Data;

[NodeData("Rasterizer", CableDataType.Mesh2D, CableDataType.RenderCommandList)]
[Slot<float, FloatEditor>("AA")]
[Slot<Camera2D>("Camera")]
public partial class RasterizerNode : NodeData<RenderCommandList>
{
    public override RenderCommandList GetTypedOutput()
    {
        var mesh = IncomingData?.GetOutput();
        if (mesh == null || mesh is not Mesh2D meshData)
            return new RenderCommandList();

        return new RenderCommandList([
            new Mesh2DRenderCommand(meshData, Camera, AA)
            ]);
    }

    public override RenderCommandList GetRenderCommands()
    {
        return GetTypedOutput();
    }
}