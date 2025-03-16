using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Data.Serialization;

public class SNode
{
    public long Id { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public SNodeType Type { get; set; }
    public Dictionary<string, object>? CollectionConstructionParameters { get; set; }
    public object? ValuesHolder { get; set; }
    public long? IncomingDataId { get; set; }
}

public class SConnection
{
    public long SourceNodeId { get; set; }
    public long TargetNodeId { get; set; }
    public string? PropertyName { get; set; }
    public string? SourcePropertyName { get; set; }
    public SConnectionType Type { get; set; }
    public bool DirectNodeConnection { get; set; }
    // TODO collection connections
}

public class SScene
{
    public string? Name { get; set; }
    public List<SNode>? Nodes { get; set; }
    public List<SConnection>? Connections { get; set; }
}

public enum SConnectionType
{
    UniformCollection,
    GenericProperty,
    Shape,
    Transform2D,
    Material,
    Camera2D
}

public enum SNodeType
{
    Timeline,
    Uniforms,
    Rectangle,
    Transform2D,
    CustomShader,
    Camera2D,
    Renderable,
    Rasterizer
}