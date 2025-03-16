using SkiaSharp;

namespace Cable.Renderer;

public class SKShaderLoader(SKShaderCompiler compiler)
{
    private readonly SKShaderCompiler _compiler = compiler;

    public void LoadShaders()
    {
        _compiler.CompileShader(@"Shaders\PerlinNoise.glsl");
        _compiler.CompileShader(@"Shaders\Tunnel.glsl");
        _compiler.CompileShader(@"Shaders\AnimatedColors.glsl");
    }
}