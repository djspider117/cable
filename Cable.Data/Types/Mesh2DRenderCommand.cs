namespace Cable.Data.Types;

public readonly struct Mesh2DRenderCommand(Mesh2D mesh, Camera2D camera, float aa) : IRenderCommand
{
    public readonly Mesh2D Mesh = mesh;
    public readonly Camera2D Camera = camera;
    public readonly float AA = aa;
}