﻿using Cable.App.Models.Data;
using Cable.App.Models.Data.Types;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Mesh2D", CableDataType.None, CableDataType.Mesh2D)]
[Slot<Geometry2D>("Geometry")]
[Slot<Matrix3x2>("Transform")]
[Slot<IMaterial>("Material")]
public partial class Mesh2DNode : NodeData<Mesh2D>
{
    public override Mesh2D GetTypedOutput()
    {
        return new Mesh2D(_geometry, _transform, _material);
    }
}