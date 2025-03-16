using SkiaSharp;

namespace Cable.Renderer;

public class SKShaderLoader(SKShaderCompiler compiler)
{
    private readonly SKShaderCompiler _compiler = compiler;

    public void LoadShaders()
    {
        _compiler.CompileShader(@"Shaders\SimplexNoise.glsl");
        _compiler.CompileShader(@"Shaders\Tunnel.glsl");
        _compiler.CompileShader(@"Shaders\AnimatedColors.glsl");
        _compiler.CompileShader(@"Shaders\BigBang.glsl");
        _compiler.CompileShader(@"Shaders\FractalPyramid.glsl");
        _compiler.CompileShader(@"Shaders\FractalTunnel.glsl");
    }
}