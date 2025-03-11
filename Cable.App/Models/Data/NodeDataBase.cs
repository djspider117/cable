using Cable.App.Models.Data.Connections;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;
using Cable.Data.Types;
using System.ComponentModel;

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

    public virtual RenderCommandList GetRenderCommands()
    {
        return new RenderCommandList([]);
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