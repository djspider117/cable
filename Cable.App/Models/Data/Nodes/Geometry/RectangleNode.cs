﻿using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Rectangle", CableDataType.None, CableDataType.RectangleShape)]
[Slot<float, FloatEditor>("Width")]
[Slot<float, FloatEditor>("Height")]
public partial class RectangleNode : NodeData<RectangleShape>
{
    public override RectangleShape GetTypedOutput()
    {
        return new(Width, Height);
        //Vector2[] vertices = [new(0, 0), new(_width, 0), new(_width, _height), new(0, _height)];
        //ushort[] indices = { 0, 1, 2, 0, 2, 3 };

        //return new Geometry2D(vertices, indices);
    }
}