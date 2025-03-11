namespace Cable.Data.Types;

public readonly struct RenderCommandList(params IRenderCommand[] commands) : ICableDataType
{
    public readonly IRenderCommand[] Commands = commands;
}
