using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData]
public unsafe partial class Transform2DNode : NodeDataBase
{
    private Matrix3x2 _transform;

    [PropertyEditor<Float2Editor>]
    private Vector2 _translation;

    [PropertyEditor<FloatEditor>]
    private float _rotation;

    [PropertyEditor<Float2Editor>]
    private Vector2 _scale = new(1, 1);

    [PropertyEditor<Float2Editor>]
    private Vector2 _center;

    public Transform2DNode()
        : base("Transform", CableDataType.Any, CableDataType.Float2)
    {
    }

    public override object? GetOutput()
    {
        var center = Center;
        _transform = Matrix3x2.CreateTranslation(Translation) *
                        Matrix3x2.CreateRotation(Rotation, center) *
                        Matrix3x2.CreateScale(Scale, center);

        var incoming = IncomingData?.GetOutput();
        if (incoming == null)
            return _transform;

        // Transform the incoming data
        if (incoming is Vector2 v)
        {
            return Vector2.Transform(v, _transform);
        }

        return null;
    }
}