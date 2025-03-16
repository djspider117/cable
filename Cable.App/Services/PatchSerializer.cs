using Cable.App.ViewModels.Data;
using Cable.Data.Serialization;
using Cable.Data.Types.MaterialData;
using Cable.Data.Types.Shaders;
using Cable.Data.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cable.App.Models.Data.Connections;

namespace Cable.App.Services;

public class PatchData
{
    public string Name { get; set; } = string.Empty;
    public long? SelectedNodeId { get; set; }

    public List<INodeData> Nodes { get; } = [];
    public List<IConnection> Connections { get; } = [];

}

public class PatchSerializer
{
    public PatchData LoadFile(string path)
    {
        var patchData = new PatchData();

        if (!File.Exists(path))
            return patchData;

        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<SScene>(json);

        if (data == null)
            return patchData;

        var scene = App.GetService<CableSceneViewModel>()!; // i don't like this

        List<(SNode, INodeData)> _postProcess = [];
        foreach (var snode in data.Nodes ?? [])
        {
            INodeData node = CreateNode(snode, scene);
            node.Id = snode.Id;
            node.X = snode.X;
            node.Y = snode.Y;

            if (snode.IncomingDataId != null)
                _postProcess.Add((snode, node));

            patchData.Nodes.Add(node);
        }
        foreach (var (snode, node) in _postProcess)
        {
            var targetNode = patchData.Nodes.Find(x => x.Id == snode.IncomingDataId!);
            if (targetNode == null)
                return patchData;

            node.IncomingData = targetNode;
        }

        foreach (var sconn in data.Connections ?? [])
        {
            var sourceNode = patchData.Nodes.Find(x => x.Id == sconn.SourceNodeId) ?? throw new InvalidDataException($"Unable to find node with id {sconn.SourceNodeId}");
            var targetNode = patchData.Nodes.Find(x => x.Id == sconn.TargetNodeId) ?? throw new InvalidDataException($"Unable to find node with id {sconn.TargetNodeId}");

            var conn = CreateConnection(sconn, sourceNode, targetNode);
            var targetPropInfo = targetNode.GetType().GetProperty(sconn.PropertyName!)!;
            targetPropInfo.SetValue(targetNode, conn);

            patchData.Connections.Add(conn);
        }

        return patchData;
    }

    public void SaveFile(string path, PatchData patchData)
    {
        // TODO
    }

    private INodeData CreateNode(SNode snode, CableSceneViewModel scene)
    {
        switch (snode.Type)
        {
            case SNodeType.Timeline:
                return new TimelineNode(scene.Timeline);
            case SNodeType.Uniforms:
                return new UniformsNode(snode.CollectionConstructionParameters ?? throw new InvalidOperationException("Cannot construct uniforms node without some uniforms."));
            case SNodeType.Rectangle:
                var rectNode = new RectangleNode();
                if (snode.ValuesHolder != null)
                {
                    var data = (RectangleShape)snode.ValuesHolder;
                    rectNode.WidthEditor.Value = data.Width;
                    rectNode.HeightEditor.Value = data.Height;
                }
                return rectNode;
            case SNodeType.Transform2D:
                var transformNode = new Transform2DNode();
                if (snode.ValuesHolder != null)
                {
                    var data = (Transform)snode.ValuesHolder;
                    transformNode.ScaleEditor.ValueX = data.Scale.X;
                    transformNode.ScaleEditor.ValueY = data.Scale.Y;
                    transformNode.RotationEditor.Value = data.Rotation;
                    transformNode.TranslationEditor.ValueX = data.Translate.X;
                    transformNode.TranslationEditor.ValueY = data.Translate.Y;
                    transformNode.CenterEditor.ValueX = data.OriginOffset.X;
                    transformNode.CenterEditor.ValueY = data.OriginOffset.Y;
                }
                return transformNode;
            case SNodeType.CustomShader:
                var customShaderNode = new CustomShaderNode();
                if (snode.ValuesHolder != null)
                {
                    var shaderData = (CustomShaderMaterialData)snode.ValuesHolder;
                    customShaderNode.SetShaderFile(new FileData(shaderData.ShaderPath));
                }
                return customShaderNode;
            case SNodeType.Camera2D:
                var cameraNode = new Camera2DNode();
                if (snode.ValuesHolder != null)
                {
                    var data = (Camera2D)snode.ValuesHolder;
                    cameraNode.ZoomEditor.Value = data.Zoom;
                    // TODO: transform
                }
                return cameraNode;
            case SNodeType.Renderable:
                return new RenderableNode();
            case SNodeType.Rasterizer:
                return new RasterizerNode();
            default:
                throw new InvalidOperationException("Unkown node type");
        }
    }

    private IConnection CreateConnection(SConnection connection, INodeData sourceNode, INodeData targetNode)
    {
        return connection.Type switch
        {
            SConnectionType.UniformCollection => new UniformCollectionConnection(sourceNode, targetNode, connection.PropertyName),
            SConnectionType.GenericProperty => new GenericPropertyConnection(sourceNode, targetNode, connection.PropertyName),
            SConnectionType.Shape => new ShapeConnection(sourceNode, targetNode, connection.PropertyName),
            SConnectionType.Transform2D => new Transform2DConnection(sourceNode, targetNode, connection.PropertyName),
            SConnectionType.Material => new MaterialConnection(sourceNode, targetNode, connection.PropertyName),
            SConnectionType.Camera2D => new Camera2DConnection(sourceNode, targetNode, connection.PropertyName),
            _ => throw new InvalidOperationException("Unkown connection type."),
        };
    }
}
