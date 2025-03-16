using SkiaSharp;

namespace Cable.Renderer;

public sealed partial class SKShaderCache : IDisposable
{
    private readonly Dictionary<string, SKRuntimeEffect> _effects = [];
    private bool _disposed;

    public void Add(string name, SKRuntimeEffect effect)
    {
        if (_disposed)
            return;

        _effects.Add(name, effect);
    }

    public SKRuntimeEffect? GetEffect(string name) => _effects.TryGetValue(name, out SKRuntimeEffect? value) ? value : null;

    public void Dispose()
    {
        _disposed = true;
        foreach (var effect in _effects.Values)
        {
            effect.Dispose();
        }

        _effects.Clear();
    }
}
