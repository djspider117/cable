using Cable.App.ViewModels.Data;
using System.Numerics;

namespace Cable.App.Models.Data.Connections;

public partial class FloatConnection(INodeData source, INodeData target) : DataConnection<float>(source, target) { }
public partial class Float2Connection(INodeData source, INodeData target) : DataConnection<Vector2>(source, target) { }