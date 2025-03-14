using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Data.Types;

public readonly record struct RectangleShape(float Width, float Height) : IShape;
public readonly record struct RoundedRectangleShape(float Width, float Height, float radius) : IShape;
public readonly record struct EllipseShape(float Width, float Height) : IShape;
public readonly record struct CircleShape(float Size) : IShape;
public readonly record struct LineShape(Vector2 start, Vector2 end, float Thickness) : IShape;