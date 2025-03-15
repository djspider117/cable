using Cable.Renderer.Exceptions;
using SkiaSharp;
using System.IO;

namespace Cable.Renderer;

public delegate void UniformSupplyAction(SKRuntimeEffectUniforms uniforms, IReadOnlyList<string> uniformNames, int uniformSize);
public sealed partial class SKShaderCompiler(SKShaderCache? shaderCache = null)
{
    private readonly SKShaderCache? _shaderCache = shaderCache;

    public SKShaderWrapper CompileShader(string shaderFileName, UniformSupplyAction uniformSupplier)
    {
        if (!File.Exists(shaderFileName))
            throw new ShaderCompilerException($"Shader file {shaderFileName} not found", new FileNotFoundException(shaderFileName));

        var shaderText = File.ReadAllText(shaderFileName);
        var shaderName = Path.GetFileNameWithoutExtension(shaderFileName);

        var cachedWrapper = _shaderCache?.GetWrapper(shaderName);
        if (cachedWrapper != null)
            return cachedWrapper.Value;

        var effect = SKRuntimeEffect.CreateShader(shaderText, out string errors)
            ?? throw new ShaderCompilerException($"Could not compile shader {shaderName}. Errors:\r\n{errors}");

        var uniforms = new SKRuntimeEffectUniforms(effect);
        uniformSupplier(uniforms, effect.Uniforms, effect.UniformSize);

        if (_shaderCache == null)
            return new SKShaderWrapper(effect.ToShader(uniforms), effect, uniforms);

        var wrapper = _shaderCache.Add(shaderName, effect, uniforms)
            ?? throw new ShaderCompilerException($"Could not add shader {shaderName} to cache");

        return wrapper;
    }
}
