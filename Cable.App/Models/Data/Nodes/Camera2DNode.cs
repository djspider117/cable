using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Camera2D", CableDataType.None, CableDataType.Geometry2D)]
[Slot<float, FloatEditor>("Zoom")]
[Slot<Matrix3x2>("Transform")]
public partial class Camera2DNode : NodeData<Camera2D>
{
    public override Camera2D GetTypedOutput()
    {
        return new Camera2D(_zoom, _transform);
    }
}

