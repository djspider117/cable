namespace Cable.Data.Types;

public readonly record struct RenderableElement(IShape? Shape, IMaterial? Material, Transform Transform) : ICableDataType;

public class RenderableCollection : List<RenderableElement>, ICableDataType;