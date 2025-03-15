using SkiaSharp;

namespace Cable.Renderer;

public sealed partial class SKShaderCache : IDisposable
{
    private readonly Dictionary<string, SKShaderWrapper> _shaderWrappers = [];
    private bool _disposed;

    public SKShaderWrapper? Add(string name, SKRuntimeEffect effect, SKRuntimeEffectUniforms uniforms)
    {
        if (_disposed)
            return null;

        var shader = effect.ToShader(uniforms);
        var wrapper = new SKShaderWrapper(shader, effect, uniforms);
        _shaderWrappers.Add(name, wrapper);

        return wrapper;
    }

    public SKShaderWrapper? GetWrapper(string name) => _shaderWrappers.ContainsKey(name) ? _shaderWrappers[name] : null;
    public SKShader? GetShader(string name) => _shaderWrappers.ContainsKey(name) ? _shaderWrappers[name].Shader : null;

    public void Dispose()
    {
        _disposed = true;
        foreach (var wrapper in _shaderWrappers.Values)
        {
            wrapper.Dispose();
        }

        _shaderWrappers.Clear();
    }
}
