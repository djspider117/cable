namespace Cable.Data.Types.MaterialData;

public readonly record struct CustomShaderMaterialData(string ShaderPath, UniformsData Uniforms, MaterialOptions MaterialOptions = default) : IMaterial;

public readonly record struct Uniform(string Name, object? Value) : ICableDataType;
public readonly record struct UniformsData(List<Uniform> Values) : ICableDataType;