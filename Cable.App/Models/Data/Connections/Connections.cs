using Cable.App.ViewModels.Data;
using Cable.Data.Types;
using Cable.Data.Types.MaterialData;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Special;
using System.Numerics;

namespace Cable.App.Models.Data.Connections;

public partial class UniformCollectionConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<UniformsData>(source, target, propName) { }
public partial class UniformValueConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Uniform>(source, target, propName) { }
public partial class StringConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<string>(source, target, propName) { }
public partial class FloatConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<float>(source, target, propName) { }
public partial class Float2Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector2>(source, target, propName) { }
public partial class Float3Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector3>(source, target, propName) { }
public partial class Float4Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vector4>(source, target, propName) { }
public partial class Geometry2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Geometry2D>(source, target, propName) { }
public partial class ShapeConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IShape>(source, target, propName) { }
public partial class Transform2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Transform>(source, target, propName) { }
public partial class MaterialConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IMaterial>(source, target, propName) { }
public partial class Camera2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Camera2D>(source, target, propName) { }
public partial class Mesh2DConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<Mesh2D>(source, target, propName) { }

public partial class UIntToFloatConnection(INodeData source, INodeData target, string? propName = null, string? sourceName = null)
    : DataConnection<float>(source, target, propName, sourceName)
{
    public override float ConvertValue(object value) => Convert.ToSingle(value);
}

public partial class GenericPropertyConnection(INodeData source, INodeData target, string? propName = null, string? sourceName = null)
    : DataConnection<object>(source, target, propName, sourceName)
{
}

public partial class GenericConnection(INodeData source, INodeData target)
    : DataConnection<object>(source, target, null, null)
{
}

public partial class ShaderInstructionConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IShaderInstruction>(source, target, propName) { }
public partial class ShaderVariableConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IVariable>(source, target, propName) { }
public partial class ShaderOperandConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IOperand>(source, target, propName) { }
public partial class ShaderExpressionConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<IExpression>(source, target, propName) { }
public partial class ShaderVec3Connection(INodeData source, INodeData target, string? propName = null) : DataConnection<Vec3Value>(source, target, propName) { }
public partial class ShaderOutputConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<OutputInstruction>(source, target, propName) { }
public partial class ShaderBuilderConnection(INodeData source, INodeData target, string? propName = null) : DataConnection<ShaderBuilder>(source, target, propName) { }