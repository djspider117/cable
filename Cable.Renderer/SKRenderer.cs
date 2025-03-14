using Cable.Data.Types;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cable.Renderer;

public class SKRenderer
{
    public event EventHandler<Vector2?>? DesiredSizeChanged;

    private readonly RendererProvider rendererProvider = new();
    private readonly ConcurrentQueue<RasterizerData> _renderQueue = new();

    private float _aa;
    private Camera2D _camera;

    private SKImageInfo? _currentFrameInfo;
    private SKSurface? _currentFrameSurface;

    public Vector2? DesiredSize { get; private set; }

    #region Setters

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetSize(Vector2 desiredSize)
    {
        DesiredSize = desiredSize;
        DesiredSizeChanged?.Invoke(this, desiredSize);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCurrentFrameInfo(SKImageInfo? curFrameInfo) => _currentFrameInfo = curFrameInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCurrentSurface(SKSurface? curSurface) => _currentFrameSurface = curSurface;

    #endregion

    public void PushFrame(RasterizerData rasterizerData)
    {
        _renderQueue.Enqueue(rasterizerData);
    }

    public void Render(SKPaintSurfaceEventArgs e)
    {
        SetCurrentSurface(e.Surface);
        SetCurrentFrameInfo(e.Info);

        if (!_renderQueue.TryDequeue(out var frameData))
            return;

        Render(e.Surface.Canvas, frameData);

        SetCurrentSurface(null);
        SetCurrentFrameInfo(null);
    }

    public void Render(SKCanvas canvas)
    {
        if (!_renderQueue.TryDequeue(out var frameData))
            return;

        Render(canvas, frameData);
    }

    private void Render(SKCanvas canvas, RasterizerData renderData)
    {
        _aa = renderData.AA;
        _camera = renderData.Camera;

        canvas.Clear(SKColors.Black);
        var updatedTransform = new Transform(_camera.Transform.Translate, _camera.Transform.Rotation, _camera.Transform.Scale * _camera.Zoom, _camera.Transform.OriginOffset);
        ApplyTransform(canvas, updatedTransform, (_currentFrameInfo?.Width ?? 0) / 2, (_currentFrameInfo?.Height ?? 0) / 2);
        
        foreach (var element in renderData.Elements)
        {
            Render(canvas, element);
        }
    }
    private void Render(SKCanvas canvas, RenderableElement elem)
    {
        if (elem.Shape is ShapeCollection col)
        {
            foreach (var shape in col)
            {
                Render(canvas, shape, elem.Material, elem.Transform);
            }
        }
        else if (elem.Shape != null)
        {
            Render(canvas, elem.Shape, elem.Material, elem.Transform);
        }
    }
    private void Render(SKCanvas canvas, IShape shape, IMaterial? material, Transform transform)
    {
        canvas.Save();
        ApplyTransform(canvas, transform);

        var renderingFunction = rendererProvider.GetRenderFunction(shape);
        renderingFunction(canvas, shape, material, transform);

        canvas.Restore();
    }

    private void ApplyTransform(SKCanvas canvas, Transform transform, float cx = 0, float cy = 0)
    {
        canvas.Translate(transform.Translate.X, transform.Translate.Y);
        canvas.RotateDegrees(transform.Rotation);
        canvas.Scale(transform.Scale.X, transform.Scale.Y, cx, cy);
    }
}
