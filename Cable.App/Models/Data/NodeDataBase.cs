using Cable.App.Models.Data.Connections;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.ComponentModel;
using System.Numerics;

namespace Cable.App.Models.Data;

public abstract class NodeDataBase : ObservableObject, INodeData
{
    public string Title { get; }
    public CableDataType InputType { get; }
    public CableDataType OutputType { get; }

    public IDataOutput? IncomingData { get; set; }

    public NodeDataBase(string title, CableDataType inType, CableDataType outType)
    {
        Title = title;
        InputType = inType;
        OutputType = outType;
    }

    public abstract IEnumerable<IPropertyEditor> GetPropertyEditors();

    public abstract object? GetOutput();

    public void SetConnection(IConnection conn)
    {
        var prop = GetType().GetProperty($"{conn.PropertyName}Connection");
        if (prop == null)
            return;

        prop.SetValue(this, conn);

        foreach (var propEditor in GetPropertyEditors())
        {
            propEditor.PushPropertyChanged();
        }
    }

    public virtual RasterizerData GetRenderCommands()
    {
        return new RasterizerData(new Camera2D(1, Transform.Identity), 0, []);
    }
}

public abstract class NodeData<T> : NodeDataBase where T : ICableDataType
{
    public NodeData(string title, CableDataType inType, CableDataType outType) 
        : base(title, inType, outType)
    {
    }

    public override object? GetOutput() => GetTypedOutput();
    public abstract T? GetTypedOutput();

}