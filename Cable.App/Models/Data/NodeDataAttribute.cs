namespace Cable.App.Models.Data;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
sealed class NodeDataAttribute : Attribute
{
	public NodeDataAttribute()
	{

	}

	public NodeDataAttribute(string name, CableDataType inType, CableDataType outType)
	{

	}
}
