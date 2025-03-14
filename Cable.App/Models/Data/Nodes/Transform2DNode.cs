using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Transform", CableDataType.Any, CableDataType.Any)]
[Slot<Vector2, Float2Editor>("Translation")]
[Slot<float, FloatEditor>("Rotation")]
[Slot<Vector2, Float2Editor>("Scale")]
[Slot<Vector2, Float2Editor>("Center")]
public partial class Transform2DNode : NodeDataBase
{
    //private Matrix3x2 _transform;

    public override object? GetOutput()
    {
        //var center = Center;
        //_transform = Matrix3x2.CreateTranslation(Translation) *
        //                Matrix3x2.CreateRotation(Rotation, center) *
        //                Matrix3x2.CreateScale(Scale, center);

        //var incoming = IncomingData?.GetOutput();
        //if (incoming == null)
        //    return _transform;

        //// Transform the incoming data
        //if (incoming is Vector2 v)
        //    return Vector2.Transform(v, _transform);

        return new Transform(Translation, Rotation, Scale, Center);
    }
}