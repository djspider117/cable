﻿using Cable.App.Models.Data;
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

public interface IConnection<T>
{
    T? GetValue();
}

public abstract partial class DataConnection<T>(INodeData source, INodeData target) : ObservableObject, IConnection<T>
{
    [ObservableProperty]
    private INodeData _sourceNode = source;

    [ObservableProperty]
    private INodeData _targetNode = target;

    public T? GetValue()
    {
        if (SourceNode == null)
            return default;

        var output = SourceNode.GetOutput();
        if (output == null)
            return default;

        return (T)output;
    }
}

public partial class FloatConnection(INodeData source, INodeData target) : DataConnection<float>(source, target) { }
public partial class Float2Connection(INodeData source, INodeData target) : DataConnection<Vector2>(source, target) { }


