using Cable.App.Models.Data;
using Cable.App.Models.Data.Types;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Rectangle", CableDataType.None, CableDataType.Geometry2D)]
[Slot<float, FloatEditor>("Width")]
[Slot<float, FloatEditor>("Height")]
public partial class RectangleNode : NodeData<Geometry2D>
{
    public override Geometry2D GetTypedOutput()
    {
        Vector2[] vertices = [new(0, 0), new(_width, 0), new(_width, _height), new(0, _height)];
        int[] indices = { 0, 1, 2, 0, 2, 3 };

        return new Geometry2D(vertices, indices);
    }
}