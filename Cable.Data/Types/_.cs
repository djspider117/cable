using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Data.Types;

public interface IShape : ICableDataType;
public class ShapeCollection : List<IShape>, IShape;

public readonly record struct RenderableElement(IShape? Shape, IMaterial? Material, Transform Transform) : ICableDataType;

public class RenderableCollection : List<RenderableElement>, ICableDataType;

public readonly record struct Transform(Vector2 Translate, float Rotation, Vector2 Scale, Vector2 origin) : ICableDataType;

public readonly record struct RectangleShape(float Width, float Height) : IShape;

