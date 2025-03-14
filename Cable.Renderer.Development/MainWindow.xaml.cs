using Cable.Data.Types;
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

namespace Cable.Renderer.Development;

public partial class MainWindow : Window
{
    private readonly SKRenderer _renderer;
    private double _time;

    public MainWindow()
    {
        InitializeComponent();

        _renderer = new SKRenderer();
        _renderer.SetSize(new Vector2(1280, 720));

        SkiaElement.Renderer = _renderer;

        _renderer.PushFrame(BuildStaticTestScene());
        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        _time++;
    }

    public RasterizerData BuildTransformTest()
    {
        var renderCol = new RenderableCollection();
        var identity = new Transform(Vector2.Zero, 0, Vector2.One, Vector2.Zero);

        var rect1 = new RectangleShape(200, 200);
        var rect2 = new RectangleShape(640, 360);

        var t1 = new Transform(new Vector2(640, 360), 0, new Vector2(1.5f, 1), new Vector2(0, 0));

        var col = new ColorMaterialData(Vector4.One);

        renderCol.Add(new RenderableElement(rect2, col, identity));
        renderCol.Add(new RenderableElement(rect1, null, t1));

        var camera = new Camera2D(1, Matrix3x2.Identity);
        return new RasterizerData(camera, 1, renderCol);
    }

    public RasterizerData BuildDemoSceneRenderTree()
    {
        var renderCol = new RenderableCollection();

        var sin = MathF.Sin((float)(_time / 200));
        var usin = sin;
        if (usin < 0)
            usin = Math.Abs(sin);

        var rect1 = new RectangleShape(200, 200);
        var rect2 = new RectangleShape(usin * 50 + 350, usin * 50 + 350);

        var c1 = Vector4.Lerp(new Vector4(1, 0, 0, 1), new Vector4(0, 1, 1, 1), usin);
        var c2 = Vector4.Lerp(new Vector4(0, 1, 0, 1), new Vector4(1, 0, 1, 1), usin);
        var c3 = Vector4.Lerp(new Vector4(0, 0, 1, 1), new Vector4(1, 1, 1, 1), usin);

        var mat1 = new ColorMaterialData(c3);
        var mat2 = new GradientMaterialData(c1, c2);

        var t1 = new Transform(new Vector2(sin * 200 + 200, sin * 500 + 100), (float)(_time / 2 % 360), Vector2.One, Vector2.Zero);
        var t2 = new Transform(new Vector2(445, 200), (float)(_time / 1.5 % 360) - 45, Vector2.One, Vector2.Zero);

        renderCol.Add(new RenderableElement(rect2, mat2, t2));
        renderCol.Add(new RenderableElement(rect1, mat1, t1));

        var camera = new Camera2D(usin + 0.5f, Matrix3x2.Identity);
        return new RasterizerData(camera, 1, renderCol);
    }

    public RasterizerData BuildStaticTestScene()
    {
        var solidColor = new ColorMaterialData(Vector4.One);

        var linGrad = new GradientMaterialData(
            new Vector4(1, 0, 1, 1),
            new Vector4(0, 1, 0, 1));

        var radGrad = new GradientMaterialData(
            new Vector4(1, 0, 1, 1),
            new Vector4(0, 1, 0, 1),
            GradientMaterialData.GradientMaterialType.Radial,
            GradientMaterialData.GradientRenderMode.Smooth,
            10);

        return new RasterizerData(
            new Camera2D(1, Matrix3x2.Identity),
            aa: 1,
            [
                                    // RECTANGLE
                new RenderableElement(
                    new RectangleShape(100, 100),
                    linGrad,
                    new Transform(new(10,10), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RectangleShape(100, 100),
                    radGrad,
                    new Transform(new(120,10), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RectangleShape(100, 100),
                    linGrad,
                    new Transform(new(340,10), 45, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RectangleShape(100, 100),
                    radGrad,
                    new Transform(new(500,10), 45, Vector2.One, Vector2.Zero)),

                                    // ROUNDED RECTANGLE
                new RenderableElement(
                    new RoundedRectangleShape(100, 100, 25),
                    linGrad,
                    new Transform(new(600,10), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RoundedRectangleShape(100, 100, 25),
                    radGrad,
                    new Transform(new(710,10), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RoundedRectangleShape(100, 100, 25),
                    linGrad,
                    new Transform(new(900,10), 45, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new RoundedRectangleShape(100, 100, 25),
                    radGrad,
                    new Transform(new(1050,10), 45, Vector2.One, Vector2.Zero)),

                                    // ELLIPSE
                new RenderableElement(
                    new EllipseShape(200, 100),
                    linGrad,
                    new Transform(new(10,180), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new EllipseShape(200, 100),
                    radGrad,
                    new Transform(new(220,180), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new CircleShape(100),
                    linGrad,
                    new Transform(new(440,180), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new CircleShape(100),
                    radGrad,
                    new Transform(new(550,180), 0, Vector2.One, Vector2.Zero)),

                                        // LINE
                new RenderableElement(
                    new LineShape(Vector2.Zero, new Vector2(100,100), 15),
                    solidColor,
                    new Transform(new(10,300), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new LineShape(Vector2.Zero, new Vector2(100,100), 15),
                    linGrad,
                    new Transform(new(110,300), 0, Vector2.One, Vector2.Zero)),

                new RenderableElement(
                    new LineShape(Vector2.Zero, new Vector2(100,100), 15),
                    radGrad,
                    new Transform(new(220,300), 0, Vector2.One, Vector2.Zero)),
            ]);
    }
}