namespace Cable.Data.Types;

public interface IShape : ICableDataType;

public class ShapeCollection : List<IShape>, IShape;