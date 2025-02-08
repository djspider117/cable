using Cable.App.ViewModels.Data;
using System.Numerics;

namespace Cable.App.Models.Data.Connections;

public partial class FloatConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<float>(source, target, propName) { }
public partial class Float2Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector2>(source, target, propName) { }