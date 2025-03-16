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

    private readonly SKRenderDispatcher rendererDispatcher;
    private readonly ConcurrentQueue<RasterizerData> _renderQueue = new();
    private readonly SKRenderPipeline _pipeline;

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

    internal SKRenderer(SKRenderPipeline pipeline)
    {
        _pipeline = pipeline;
        rendererDispatcher = new SKRenderDispatcher(pipeline);
    }

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
        //_aa = renderData.AA;
        _camera = renderData.Camera;

        var rcontext = new SKRenderContext(canvas, renderData.Time);
        
        canvas.Clear(SKColors.Black);
        var updatedTransform = new Transform(_camera.Transform.Translate, _camera.Transform.Rotation, _camera.Transform.Scale * _camera.Zoom, _camera.Transform.OriginOffset);
        ApplyTransform(rcontext, updatedTransform, (_currentFrameInfo?.Width ?? 0) / 2, (_currentFrameInfo?.Height ?? 0) / 2);

        foreach (var element in renderData.Elements)
        {
            Render(rcontext, element);
        }
    }
    private void Render(SKRenderContext context, RenderableElement elem)
    {
        if (elem.Shape is ShapeCollection col)
        {
            foreach (var shape in col)
            {
                Render(context, shape, elem.Material, elem.Transform);
            }
        }
        else if (elem.Shape != null)
        {
            Render(context, elem.Shape, elem.Material, elem.Transform);
        }
    }
    private void Render(SKRenderContext context, IShape shape, IMaterial? material, Transform transform)
    {
        context.Canvas.Save();
        ApplyTransform(context, transform);

        rendererDispatcher.Render(context, shape, material, transform);
        
        context.Canvas.Restore();
    }

    private void ApplyTransform(SKRenderContext context, Transform transform, float cx = 0, float cy = 0)
    {
        context.Canvas.Translate(transform.Translate.X, transform.Translate.Y);
        context.Canvas.RotateDegrees(transform.Rotation);
        context.Canvas.Scale(transform.Scale.X, transform.Scale.Y, cx, cy);
    }
}
