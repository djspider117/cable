using Codeuctivity.SkiaSharpCompare;
using SkiaSharp;
using System.Numerics;

namespace Cable.Renderer.Tests;

[TestClass]
public sealed class SKRendererTests
{
    [TestMethod]
    public void StaticSceneTest_ShouldMatchReference()
    {
        var scene = StaticSceneBuilder.BuildScene();

        var _renderer = new SKRenderer();
        _renderer.SetSize(new Vector2(1280, 720));
        _renderer.PushFrame(scene);

        var info = new SKImageInfo(1280, 720, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info);

        _renderer.SetCurrentFrameInfo(info);
        _renderer.SetCurrentSurface(surface);
        _renderer.Render(surface.Canvas);

        using var snapshot = surface.Snapshot();
        using var data = snapshot.Encode(SKEncodedImageFormat.Png, 100);
        using (var stream = File.OpenWrite("actual.png"))
            data.SaveTo(stream);

        var calcDiff = Compare.CalcDiff("actual.png", @"Data\RendererReference.png");

        Assert.AreEqual(0, calcDiff.AbsoluteError);
    }
}
