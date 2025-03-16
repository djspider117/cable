using Cable.App.Models.Data;
using Cable.App.Models.Data.Connections;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types.MaterialData;
using Cable.Data.Types.Shaders.Special;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata;

namespace Cable.App.ViewModels.Data;

[NodeData("Custom Shader", CableDataType.None, CableDataType.Material)]
[Slot<ShaderBuilder>("ShaderBuilder")]
[Slot<FileData>("ShaderFile")]
[Slot<UniformsData>("Uniforms")]
public partial class CustomShaderNode : NodeData<IMaterial>
{
    private bool _hasShaderContentChanged = true;
    private string _customShaderPath;

    public void SetShaderFile(FileData shaderFile) => _shaderFile = shaderFile;
    public void SetShaderBuilder(ShaderBuilder builder) => _shaderBuilder = builder;

    public override IMaterial? GetTypedOutput()
    {
        if (ShaderBuilder != null)
        {
            if (_hasShaderContentChanged)
            {
                _customShaderPath = $@"ShaderAutoGen\{Guid.NewGuid()}.glsl";
                var shaderText = ShaderBuilder.BuildShader();

                var dirName = Path.GetDirectoryName(_customShaderPath)!;
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                File.WriteAllText(_customShaderPath, shaderText);

                _hasShaderContentChanged = false;
            }

            return new CustomShaderMaterialData(_customShaderPath, Uniforms);
        }

        if (!string.IsNullOrEmpty(ShaderFile.Path))
        {
            return new CustomShaderMaterialData(ShaderFile.Path, Uniforms);
        }

        return ColorMaterialData.InvalidColor;
    }
}


public partial class UniformsNode : NodeData<UniformsData>
{
    private readonly List<UniformEditor> _editors = [];
    private readonly Dictionary<string, IConnection<object>?> _connections = [];

    public ObservableCollection<UniformRow> Uniforms { get; set; } = [];

    public UniformsNode(Dictionary<string, object> inputs) 
        : base("Uniforms", CableDataType.None, CableDataType.Uniforms)
    {
        foreach (var kvp in inputs)
        {
            var row = new UniformRow { Name = kvp.Key, Value = kvp.Value };
            Uniforms.Add(row);
            _editors.Add(new UniformEditor(this, kvp.Key, row.GetData, row.SetData));
        }
    }

    public void AddConnectionForCollection(string target, IConnection<object>? connection)
    {
        if (_connections.ContainsKey(target))
            _connections[target] = connection;
        else
            _connections.Add(target, connection);

        var editor = _editors.Find(x => x.DisplayName == target);
        if (editor == null)
            return;

        editor.IsConnected = connection != null;
        if (connection != null)
        {
            editor.DataGetter = () => new Uniform(target, connection.GetValue());
        }
    }

    public override UniformsData GetTypedOutput()
    {
        List<Uniform> unis = [];
        foreach (var item in _editors)
        {
            if (item.DataGetter == null)
                continue;

            unis.Add((Uniform)item.DataGetter());
        }

        return new UniformsData(unis);
    }

    public override IEnumerable<IPropertyEditor> GetPropertyEditors() => _editors;
}

public partial class UniformRow : ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private object? _value;

    public Uniform GetData() => new(Name ?? Guid.NewGuid().ToString(), Value);

    internal void SetData(Uniform uni)
    {
        Name = uni.Name;
        Value = uni.Value;
    }
}