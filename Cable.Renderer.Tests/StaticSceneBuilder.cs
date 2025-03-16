using Cable.Data.Types;
using Cable.Data.Types.MaterialData;
using System.Numerics;

namespace Cable.Renderer.Tests;

public class StaticSceneBuilder
{
    public static RasterizerData BuildScene()
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
            new Camera2D(1, Transform.Identity),
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
