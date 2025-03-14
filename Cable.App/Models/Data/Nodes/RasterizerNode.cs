using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;

namespace Cable.App.ViewModels.Data;

[NodeData("Rasterizer", CableDataType.Renderable, CableDataType.RenderCommandList)]
[Slot<float, FloatEditor>("AA")]
[Slot<Camera2D>("Camera")]
public partial class RasterizerNode : NodeData<RasterizerData>
{
    public override RasterizerData GetTypedOutput()
    {
        var input = IncomingData?.GetOutput();
        if (input == null)
            return new RasterizerData();

        if (input is RenderableElement elem)
            return new RasterizerData(Camera, AA, [elem]);

        if (input is RenderableCollection col)
            return new RasterizerData(Camera, AA, col);

        throw new InvalidOperationException();
    }

    public override RasterizerData GetRenderCommands()
    {
        return GetTypedOutput();
    }
}