using Cable.App.ViewModels.Data;
using Cable.Data.Types;
using System.Numerics;

namespace Cable.App.Models.Data.Connections;

public partial class FloatConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<float>(source, target, propName) { }
public partial class Float2Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector2>(source, target, propName) { }
public partial class Float3Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector3>(source, target, propName) { }
public partial class Float4Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector4>(source, target, propName) { }
public partial class Geometry2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Geometry2D>(source, target, propName) { }
public partial class Transform2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Matrix3x2>(source, target, propName) { }
public partial class MaterialConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IMaterial>(source, target, propName) { }
public partial class Camera2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Camera2D>(source, target, propName) { }
public partial class Mesh2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Mesh2D>(source, target, propName) { }
