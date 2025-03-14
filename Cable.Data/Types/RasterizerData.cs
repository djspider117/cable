namespace Cable.Data.Types;

public readonly struct RasterizerData(Camera2D camera, float aa, RenderableCollection elements) : ICableDataType
{
    public readonly RenderableCollection Elements = elements;
    public readonly Camera2D Camera = camera;
    public readonly float AA = aa;
}
