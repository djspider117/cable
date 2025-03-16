using Cable.Renderer.Exceptions;
using SkiaSharp;
using System.IO;

namespace Cable.Renderer;

public sealed partial class SKShaderCompiler(SKShaderCache? shaderCache = null)
{
    private readonly SKShaderCache? _shaderCache = shaderCache;

    public SKRuntimeEffect CompileShader(string shaderFileName)
    {
        if (!File.Exists(shaderFileName))
            throw new ShaderCompilerException($"Shader file {shaderFileName} not found", new FileNotFoundException(shaderFileName));

        var shaderText = File.ReadAllText(shaderFileName);
        var shaderName = Path.GetFileNameWithoutExtension(shaderFileName);

        var cached = _shaderCache?.GetEffect(shaderName);
        if (cached != null)
            return cached;

        var effect = SKRuntimeEffect.CreateShader(shaderText, out string errors)
            ?? throw new ShaderCompilerException($"Could not compile shader {shaderName}. Errors:\r\n{errors}");

        if (_shaderCache == null)
            return effect;

        _shaderCache.Add(shaderName, effect);

        return effect;
    }
}
