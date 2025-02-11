using Cable.App.Models.Data;
using Cable.App.Models.Data.Types;
using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.ViewModels.Data;

[NodeData("Rasterizer", CableDataType.Mesh2D, CableDataType.TextureRGBA)]
[Slot<float, FloatEditor>("AA")]
[Slot<Camera2D>("Camera")]
public partial class RasterizerNode : NodeData<TextureRGBA>
{
    public override TextureRGBA GetTypedOutput()
    {
        var mesh = IncomingData?.GetOutput();
        if (mesh == null)
            return new TextureRGBA();

        // TODO render
        return new TextureRGBA();
    }
}