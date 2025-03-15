namespace Cable.Renderer;

public sealed partial class SKRenderPipeline : IDisposable
{
    public SKShaderCache ShaderCache { get; init; }
    public SKShaderCompiler Compiler {get; init; }
    public SKShaderLoader ShaderLoader { get; init; }
    public SKPaintProvider PaintProvider { get; init; }
    public SKRenderer Renderer { get; init; }

    public SKRenderPipeline()
    {
        ShaderCache = new SKShaderCache();
        Compiler = new SKShaderCompiler(ShaderCache);
        ShaderLoader = new SKShaderLoader(Compiler);
        PaintProvider = new SKPaintProvider(ShaderCache);
        Renderer = new SKRenderer(this);
    }

    public void Initialize()
    {
        ShaderLoader.LoadShaders();
    }

    public void Dispose()
    {
        ShaderCache.Dispose();
    }
}
