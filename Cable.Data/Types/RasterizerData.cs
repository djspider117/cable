namespace Cable.Data.Types;

public readonly struct RasterizerData(Camera2D camera, float aa, RenderableCollection elements, float time = 0) : ICableDataType
{
    public readonly RenderableCollection Elements = elements;
    public readonly Camera2D Camera = camera;
    public readonly float AA = aa;
    public readonly float Time = time;
}
