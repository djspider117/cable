using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types.Shaders;

namespace Cable.App.Models.Data.Nodes.Shaders;

[NodeData("Vector Variant", CableDataType.None, CableDataType.ShaderInstruction)]
[Slot<string, StringEditor>("Pattern")]
[Slot<IVariable>("Input")]
public partial class VectorVariantNode : NodeData<VectorVariant>
{
    public override VectorVariant? GetTypedOutput()
    {
        return new VectorVariant { Pattern = Pattern, Input = Input };
    }
}
