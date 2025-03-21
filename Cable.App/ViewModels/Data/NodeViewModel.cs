﻿using Cable.App.Models.Data;
using Cable.App.Models.Data.Connections;
using Cable.App.Models.Data.Nodes;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data;

namespace Cable.App.ViewModels.Data;

public partial class NodeViewModel : ObservableObject
{
    private readonly NodeDataBase _data;

    [ObservableProperty]
    private double _x = 0d;

    [ObservableProperty]
    private double _y = 0d;

    [ObservableProperty]
    private bool _isSelected = false;

    public NodeDataBase Data => _data;
    public string Title => _data.Title;
    public CableDataType InputType => _data.InputType;
    public CableDataType OutputType => _data.OutputType;

    public IEnumerable<IPropertyEditor> PropertyEditors { get; set; }

    public NodeViewModel()
    {
        _data = new EmptyNode();
        PropertyEditors = [];
    }

    public NodeViewModel(INodeData data)
    {
        _data = (NodeDataBase)data;
        X = data.X;
        Y = data.Y;
        PropertyEditors = data.GetPropertyEditors();
    }

    public NodeViewModel(NodeDataBase data)
    {
        _data = data;
        X = data.X;
        Y = data.Y;
        PropertyEditors = data.GetPropertyEditors();
    }
}


public partial class ConnectionViewModel(NodeViewModel? sourceNode, NodeViewModel? targetNode, IConnection? connection) : ObservableObject
{
    [ObservableProperty]
    private IConnection? _connection = connection;

    [ObservableProperty]
    private NodeViewModel? _sourceNode = sourceNode;

    [ObservableProperty]
    private NodeViewModel? _targetNode = targetNode;
}