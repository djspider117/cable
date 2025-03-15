using SkiaSharp;

namespace Cable.Renderer;

public readonly struct SKShaderWrapper(SKShader shader, SKRuntimeEffect effect, SKRuntimeEffectUniforms uniforms) : IDisposable
{
    public readonly SKShader Shader = shader;
    public readonly SKRuntimeEffect Effect = effect;
    public readonly SKRuntimeEffectUniforms Uniforms = uniforms;

    public void Dispose()
    {
        Effect?.Dispose();
        Uniforms.Dispose();
        Shader?.Dispose();
    }
}
