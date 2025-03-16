using Cable.Data;
using Cable.Data.Types.Shaders.Special;

namespace Cable.App.Models.Data.Nodes.Shaders;

[NodeData("Shader Builder", CableDataType.None, CableDataType.ShaderBuilder)]
[Slot<OutputInstruction>("ShaderOutput")]
public partial class ShaderBuilderNode : NodeData<ShaderBuilder>
{
    public override ShaderBuilder? GetTypedOutput()
    {
        // TODO, these will have to be declared in the node editor not in the graph
        // there is currently no mechanism to edit in the node editor so we hardcode for now

        return new ShaderBuilder
        {
            Uniforms =
            [
                new() { Type = ShaderValueType.Float, Name = "iTime" },
                new() { Type = ShaderValueType.Vector2, Name = "iResolution" },
            ],
            Output = ShaderOutput
        };
    }
}
