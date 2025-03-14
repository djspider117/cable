using Cable.App.Models.Data;
using Cable.App.Models.Data.Connections;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;

namespace Cable.App.ViewModels.Data;

public partial class TimelineNode : NodeDataBase
{
    private readonly CableTimeline _timeline;
    private IConnection<uint>? _frameIndexConnection;

    public uint FrameIndex => _timeline.FrameIndex;
    private FloatOutputEditor? _frameIndexEditor;

    public IConnection<uint>? FrameIndexConnection
    {
        get => _frameIndexConnection;
        set
        {
            _frameIndexConnection = value;
            if (_frameIndexConnection != null)
            {
                _frameIndexConnection.PropertyChanged += FrameIndexConnection_PropertyChanged;
                OnPropertyChanged();
            }
        }
    }
    public FloatOutputEditor FrameIndexEditor
    {
        get
        {
            if (_frameIndexEditor == null)
                _frameIndexEditor = new FloatOutputEditor(this, "FrameIndex", () => _timeline.FrameIndex);
            return _frameIndexEditor;
        }
    }

    private void FrameIndexConnection_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(FrameIndex));
    }


    public override IEnumerable<IPropertyEditor> GetPropertyEditors()
    {
        return [FrameIndexEditor];
    }

    public TimelineNode(CableTimeline timeline) : base("Timeline", (Cable.Data.CableDataType)0, (Cable.Data.CableDataType)11)
    {
        _timeline = timeline;
    }

    public override object? GetOutput()
    {
        return _timeline;
    }

    public override object? GetPropertyOutput(string propertyName)
    {
        if (propertyName == nameof(FrameIndex))
            return FrameIndex;

        return null;
    }
}

public partial class FloatOutputEditor(INodeData sourceNode, string name, Func<float> getter) : OutputPropertyEditor<float>(sourceNode, name, getter)
{
    public override IConnection CreateConnectionAsDestination(INodeData source) => new FloatConnection(source, Parent, DisplayName);
    public override IConnection CreateConnectionAsSource(INodeData destination) => new FloatConnection(Parent, destination, DisplayName);
}