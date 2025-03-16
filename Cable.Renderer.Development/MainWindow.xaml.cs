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

namespace Cable.Renderer.Development;

public partial class MainWindow : Window
{
    private SKRenderPipeline _pipeline;
    private readonly SKRenderer _renderer;
    private double _time;
    private int _index = 1;

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

    public RasterizerData BuildShaderScene()
    {
        var renderCol = new RenderableCollection();

        var bgRect = new RectangleShape(1280, 720);

        var mat3 = new ScrollingColorsMaterialData();

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