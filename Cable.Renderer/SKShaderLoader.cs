using SkiaSharp;

namespace Cable.Renderer;

public class SKShaderLoader(SKShaderCompiler compiler)
{
    private readonly SKShaderCompiler _compiler = compiler;

    public void LoadShaders()
    {
        _compiler.CompileShader(@"Shaders\PerlinNoise.glsl", StandardUniformSupplier);
        _compiler.CompileShader(@"Shaders\Tunnel.glsl", StandardUniformSupplier);
        _compiler.CompileShader(@"Shaders\AnimatedColors.glsl", StandardUniformSupplier);
    }

    private void StandardUniformSupplier(SKRuntimeEffectUniforms uniforms, IReadOnlyList<string> uniformNames, int uniformSize)
    {
        uniforms.Add("iTime", 0f);
        uniforms.Add("iResolution", new SKSize(100, 100));
    }
}