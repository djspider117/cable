using Cable.Data.Types;
using Cable.Renderer.Tests;
using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Transform = Cable.Data.Types.Transform;
using System.IO;
using System.Windows.Threading;
using Cable.Data.Types.MaterialData;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Math;
using Cable.Data.Types.Shaders.Special;
using Cable.Data.Types.Shaders.Predefined;

namespace Cable.Renderer.Development;

public partial class MainWindow : Window
{
    private SKRenderPipeline _pipeline;
    private readonly SKRenderer _renderer;
    private double _time;
    private int _index = 0;
    private VariableNameGenerator _nameGen = new();

    public MainWindow()
    {
        _pipeline = new SKRenderPipeline();
        _pipeline.Initialize();

        InitializeComponent();

        _renderer = _pipeline.Renderer;
        _renderer.SetSize(new Vector2(1280, 720));

        SkiaElement.Renderer = _renderer;

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _pipeline.Dispose();
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        _time++;
        Render();
        SkiaElement.InvalidateVisual();
    }

    private void Render()
    {
        if (_index == 0)
            _renderer.PushFrame(BuildShaderScene());

        if (_index == 1)
            _renderer.PushFrame(BuildRnDScene());

        if (_index == 2)
            _renderer.PushFrame(BuildTransformTest());

        if (_index == 3)
            _renderer.PushFrame(StaticSceneBuilder.BuildScene());
    }
    private void BuildReferenceImage()
    {
        var info = new SKImageInfo(1280, 720, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info);

        _renderer.SetCurrentFrameInfo(info);
        _renderer.SetCurrentSurface(surface);
        _renderer.Render(surface.Canvas);

        using var snapshot = surface.Snapshot();
        using var data = snapshot.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite("reference.png");
        data.SaveTo(stream);
    }

    #region Scenes

    public ShaderBuilder CreateShaderBuilder()
    {

        Vec3Value v3_1 = new Vec3Declaration() { VariableName = _nameGen.CreateVariable(), Value = new Vector3(0, 2, 4) };
        VectorVariant uv_xyx = new() { Pattern = "xyx", Input = UVValue.Instance };
        Add add_1 = new() { Operands = [TimeValue.Instance, uv_xyx, v3_1] };

        Vec3Declaration add_declr = new() { VariableName = _nameGen.CreateVariable(), Expression = add_1 };

        FloatValue floatVal = new() { Value = 0.5f };
        Cos cos = new() { Expression = add_declr };

        Mul mul_1 = new() { Operands = [floatVal, cos] };
        Vec3Declaration mul_declr = new() { VariableName = _nameGen.CreateVariable(), Expression = mul_1 };

        Add add_2 = new() { Operands = [floatVal, mul_declr] };

        Vec3Value v3_2 = new() { Expression = add_2 };
        Vec3Declaration v3_decl = new() { VariableName = _nameGen.CreateVariable(), Vec3Reference = v3_2 };

        var builder = new ShaderBuilder
        {
            Uniforms =
            [
                new() { Type = ShaderValueType.Float, Name = "iTime" },
                new() { Type = ShaderValueType.Vector2, Name = "iResolution" },
            ],
            Output = new OutputInstruction() { InputVec3 = v3_decl, OutputAlpha = 0 }
        };

        return builder;
    }

    private bool _hasShaderContentChanged = true;
    private string _customShaderPath = string.Empty;

    public RasterizerData BuildShaderScene()
    {
        // ----------- This section will be handled in the node that returns CustomShaderMaterialData ------------

        if (_hasShaderContentChanged)
        {
            var shaderBuilder = CreateShaderBuilder();
            _customShaderPath = $@"ShaderAutoGen\{Guid.NewGuid()}.glsl";
            var shaderText = shaderBuilder.BuildShader();

            var dirName = System.IO.Path.GetDirectoryName(_customShaderPath)!;
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            File.WriteAllText(_customShaderPath, shaderText);

            _hasShaderContentChanged = false;
        }

        //--------------------------------------------------------------------------------------------------------

        var renderCol = new RenderableCollection();

        var bgRect = new RectangleShape(1280, 720);

        //var mat3 = new CustomShaderMaterialData(
        //    @"Shaders\SimplexNoise.glsl",
        //    new()
        //    {
        //        { "iOffset", new Vector2((float)(_time / 60f) / 20f, 0) } 
        //    }
        // );

        var mat3 = new CustomShaderMaterialData(_customShaderPath, new UniformsData([]));


        renderCol.Add(new RenderableElement(bgRect, mat3, Transform.Identity));

        var camera = new Camera2D(1, Transform.Identity);
        return new RasterizerData(camera, 1, renderCol, (float)(_time / 60f));
    }

    public RasterizerData BuildRnDScene()
    {
        var renderCol = new RenderableCollection();

        var sin = MathF.Sin((float)(_time / 200));
        var usin = sin;
        if (usin < 0)
            usin = Math.Abs(sin);

        var bgRect = new RectangleShape(1280, 720);
        var rect1 = new RectangleShape(200, 200);
        var rect2 = new RectangleShape(usin * 50 + 350, usin * 50 + 350);

        var c1 = Vector4.Lerp(new Vector4(1, 0, 0, 1), new Vector4(0, 1, 1, 1), usin);
        var c2 = Vector4.Lerp(new Vector4(0, 1, 0, 1), new Vector4(1, 0, 1, 1), usin);
        var c3 = Vector4.Lerp(new Vector4(0, 0, 1, 1), new Vector4(1, 1, 1, 1), usin);

        var mat1 = new ColorMaterialData(c3);
        var mat2 = new GradientMaterialData(c1, c2);
        var mat3 = new NoiseMaterialData();

        var t1 = new Transform(new Vector2(sin * 200 + 200, sin * 500 + 100), (float)(_time / 2 % 360), Vector2.One, Vector2.Zero);
        var t2 = new Transform(new Vector2(445, 200), (float)(_time / 1.5 % 360) - 45, Vector2.One, Vector2.Zero);

        renderCol.Add(new RenderableElement(bgRect, mat3, Transform.Identity));
        renderCol.Add(new RenderableElement(rect2, mat2, t2));
        renderCol.Add(new RenderableElement(rect1, mat1, t1));

        var camera = new Camera2D(usin / 2 + 0.5f, Transform.Identity);
        return new RasterizerData(camera, 1, renderCol, (float)(_time / 60f));
    }

    public RasterizerData BuildTransformTest()
    {
        var sin = MathF.Sin((float)(_time / 200));
        var usin = sin;
        if (usin < 0)
            usin = Math.Abs(sin);


        var renderCol = new RenderableCollection();
        var identity = new Transform(Vector2.Zero, 0, Vector2.One, Vector2.Zero);

        var rect1 = new RectangleShape(200, 200);
        var rect2 = new RectangleShape(1280, 720);

        var t1 = new Transform(new Vector2(640, 360), 0, new Vector2(1.5f, 1), new Vector2(0, 0));

        var col = new ColorMaterialData(Vector4.One);

        renderCol.Add(new RenderableElement(rect2, col, identity));
        renderCol.Add(new RenderableElement(rect1, null, t1));

        var camera = new Camera2D(usin + 0.0001f, Transform.Identity);
        return new RasterizerData(camera, 1, renderCol, (float)(_time / 60f));
    }

    #endregion

    private void btnSceneCycle_Click(object sender, RoutedEventArgs e)
    {
        _index = (_index + 1) % 4;
    }
    private void btnSaveReference_Click(object sender, RoutedEventArgs e)
    {
        BuildReferenceImage();
    }
}