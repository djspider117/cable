using Cable.App.Models.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types.MaterialData;
using System.Numerics;

namespace Cable.App.ViewModels.Data;

[NodeData]
[Slot<Vector4, ColorEditor>("Color1")]
[Slot<Vector4, ColorEditor>("Color2")]
public partial class GradienteNode : NodeData<IMaterial>
{
    public GradienteNode()
        : base("Gradient", CableDataType.None, CableDataType.Material)
    {
        //TODO: move this back to attribute, thisis for testing
        _color1 = new Vector4(1, 0, 1, 1);
        _color2 = new Vector4(0, 1, 0, 1);
    }

    public override IMaterial GetTypedOutput()
    {
        return new GradientMaterialData(_color1, _color2);
    }
}
