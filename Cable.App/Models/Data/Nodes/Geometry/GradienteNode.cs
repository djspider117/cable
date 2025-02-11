using Cable.App.Models.Data;
using Cable.App.Models.Data.Types;
using Cable.App.ViewModels.Data.PropertyEditors;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData("Gradient", CableDataType.None, CableDataType.Material)]
[Slot<Vector4, Float4Editor>("Color1")]
[Slot<Vector4, Float4Editor>("Color2")]
public partial class GradienteNode : NodeData<IMaterial>
{
    public override IMaterial GetTypedOutput()
    {
        return new GradientMaterialData(_color1, _color2);
    }
}
