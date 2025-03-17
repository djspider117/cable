using Cable.App.Extensions;
using Cable.App.Models.Data;
using Cable.App.Models.Data.Connections;
using Cable.App.Models.Data.Nodes.Shaders;
using Cable.App.Models.Messages;
using Cable.App.Services;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Core;
using Cable.Data.Serialization;
using Cable.Data.Types;
using Cable.Data.Types.MaterialData;
using Cable.Data.Types.Shaders;
using Cable.Data.Types.Shaders.Special;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Cable.App.Views.Controls;

public partial class GraphEditor : UserControl, INodeViewResolver
{
    #region PropDP

    public static readonly DependencyProperty SelectedNodeViewProperty =
        DependencyProperty.Register(nameof(SelectedNodeView), typeof(NodeView), typeof(GraphEditor), new PropertyMetadata(null));

    #endregion

    #region Fields

    private readonly Dictionary<NodeViewModel, NodeView> _nodeMapping = [];
    private NodeConnectionView? _pendingConnection;
    private Point? _lastMove;

    private List<NodeView> _nodeViews = [];
    private List<NodeConnectionView> _connectionViews = [];

    #endregion

    #region Properties

    public NodeView? SelectedNodeView
    {
        get => (NodeView)GetValue(SelectedNodeViewProperty);
        set => SetValue(SelectedNodeViewProperty, value);
    }

    public ObservableCollection<NodeViewModel> NodeViewModels { get; init; } = [];
    public ObservableCollection<ConnectionViewModel> ConnectionViewModels { get; init; } = [];

    #endregion

    public GraphEditor()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<StartNodeConnectionMessage>(this, OnStartNodeConnectionMessage);
        Application.Current.MainWindow.MouseUp += MainWindow_MouseUp;

        NodeViewModels.CollectionChanged += NodeViewModels_CollectionChanged;
        ConnectionViewModels.CollectionChanged += ConnectionViewModels_CollectionChanged;

        LoadPatch(DemoPatches.BuildDemo());

        Loaded += GraphEditor_Loaded;
    }

    #region Event Handlers

    private async void GraphEditor_Loaded(object sender, RoutedEventArgs e)
    {
        await Task.Delay(100);
        PerformAutoLayout();
    }

    private void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is FrameworkElement fe && fe.Parent is DataConnector dc)
        {
            var nv = dc.FindVisualParent<NodeView>()!;
            if (_pendingConnection != null)
            {
                var editor = (dc.DataContext as IPropertyEditor)!;
                var conn = editor.CreateConnectionAsDestination(_pendingConnection.Source.ViewModel.Data);

                nv.ViewModel.Data.SetConnection(conn);

                _pendingConnection.Destination = nv;
                _pendingConnection.Connection = conn;
                _pendingConnection.InvalidateVisual();
                _pendingConnection = null;
            }
        }

        if (e.ChangedButton == MouseButton.Left)
        {
            if (e.OriginalSource is FrameworkElement fe2)
            {
                var nv = fe2.FindVisualParent<NodeView>()!;
                if (_pendingConnection == null)
                {
                    Select(nv);
                }
            }
        }

        if (_pendingConnection?.Destination == null)
            pnlConnectionsContainer.Children.Remove(_pendingConnection);
    }

    private void btnPerformLayout_Click(object sender, RoutedEventArgs e)
    {
        PerformAutoLayout();
    }

    #endregion

    #region Selection

    public void Select(NodeViewModel? vm)
    {
        var nv = GetViewFromViewModel(vm);
        Select(nv);
    }

    public void Select(NodeView? nv)
    {
        if (SelectedNodeView != null)
            SelectedNodeView.ViewModel.IsSelected = false;

        SelectedNodeView = nv;
        monitor.NodeToPreview = nv;

        if (nv != null)
            nv.ViewModel.IsSelected = true;
    }

    #endregion

    #region Overrides
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (e.MiddleButton == MouseButtonState.Pressed && _lastMove != null)
        {
            var delta = e.GetPosition(this) - _lastMove;
            connTranslateTransform.X += delta.Value.X;
            connTranslateTransform.Y += delta.Value.Y;

            nodeTranslateTransform.X += delta.Value.X;
            nodeTranslateTransform.Y += delta.Value.Y;
           
        }

        _lastMove = e.GetPosition(this);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        var deltaVal = e.Delta / 1400.0d;

        var pos = e.GetPosition(this);
        connScaleTransform.CenterX = pos.X;
        connScaleTransform.CenterY = pos.Y;
        nodeScaleTransform.CenterX = pos.X;
        nodeScaleTransform.CenterY = pos.Y;

        connScaleTransform.ScaleX += deltaVal;
        connScaleTransform.ScaleY += deltaVal;
        nodeScaleTransform.ScaleX += deltaVal;
        nodeScaleTransform.ScaleY += deltaVal;
    }

    #endregion

    #region Save/Load

    private void NodeViewModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (NodeViewModel vm in e.NewItems)
            {
                var view = new NodeView() { ViewModel = vm };
                _nodeViews.Add(view);
                pnlNodeContainer.Children.Add(view);
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
        {
            foreach (NodeViewModel vm in e.OldItems)
            {
                // TODO
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            // TODO
        }


    }
    private void ConnectionViewModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (ConnectionViewModel vm in e.NewItems)
            {
                var connView = new NodeConnectionView(vm, this);
                pnlConnectionsContainer.Children.Add(connView);
                _connectionViews.Add(connView);
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
        {
            foreach (NodeViewModel vm in e.OldItems)
            {
                // TODO
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            // TODO
        }
    }

    public void LoadPatch(PatchData patchData)
    {
        var nodeVms = new Dictionary<long, NodeViewModel>();
        foreach (var node in patchData.Nodes)
        {
            var vm = new NodeViewModel(node);
            NodeViewModels.Add(vm);
            nodeVms.Add(node.Id, vm);
        }

        foreach (var conn in patchData.Connections)
        {
            var srcvm = nodeVms[conn.SourceNode.Id];
            var trgvm = nodeVms[conn.TargetNode.Id];
            var cvm = new ConnectionViewModel(srcvm, trgvm, conn);
            ConnectionViewModels.Add(cvm);
        }

        if (patchData.SelectedNodeId is not null)
        {
            Select(nodeVms[patchData.SelectedNodeId.Value]);
        }
    }

    public PatchData SavePatch()
    {
        var rv = new PatchData();

        // TODO

        return rv;
    }

    #endregion

    #region Layout

    public void PerformAutoLayout()
    {
        Graph<NodeView> graph = GetNodeViewGraph();

        if (graph.Root == null)
            return;

        var layout = new GraphLayout<NodeView>();
        layout.Layout(graph.Root, 0, ActualWidth - 650);
    }
    public Graph<NodeView> GetNodeViewGraph()
    {
        var nodes = new List<GraphItem<NodeView>>();

        foreach (NodeConnectionView conn in pnlConnectionsContainer.Children)
        {
            var existingSrc = nodes.FirstOrDefault(x => x.Data == conn.Source);
            var existingDst = nodes.FirstOrDefault(x => x.Data == conn.Destination);

            if (existingSrc != null && existingDst != null)
            {
                existingSrc.Children.Add(existingDst);
                existingDst.Parents.Add(existingSrc);
            }

            if (existingSrc == null && existingDst != null)
            {
                var srcNode = new GraphItem<NodeView> { Data = conn.Source };
                srcNode.Children.Add(existingDst);
                existingDst.Parents.Add(srcNode);
                nodes.Add(srcNode);
            }

            if (existingSrc != null && existingDst == null && conn.Destination != null)
            {
                var dstNode = new GraphItem<NodeView> { Data = conn.Destination };
                existingSrc.Children.Add(dstNode);
                dstNode.Parents.Add(existingSrc);

                nodes.Add(dstNode);
            }

            if (existingSrc == null && existingDst == null && conn.Destination != null)
            {
                var srcNode = new GraphItem<NodeView> { Data = conn.Source };
                var dstNode = new GraphItem<NodeView> { Data = conn.Destination };

                srcNode.Children.Add(dstNode);
                dstNode.Parents.Add(srcNode);

                nodes.Add(srcNode);
                nodes.Add(dstNode);
            }
        }

        // the node with the least outputs is going to be our root
        var root = nodes.OrderBy(x => x.Children.Count).FirstOrDefault();
        return new Graph<NodeView> { Root = root };
    }

    public NodeView? GetViewFromViewModel(NodeViewModel? vm)
    {
        return _nodeViews.Find(x => x.ViewModel == vm);
    }

    #endregion

    #region Message Handlers

    private void OnStartNodeConnectionMessage(object recipient, StartNodeConnectionMessage message)
    {
        _pendingConnection = new NodeConnectionView(message.SourceView, null, null!);
        pnlConnectionsContainer.Children.Add(_pendingConnection);
    }

    #endregion
}

public class DemoPatches
{
    public static PatchData BuildDemo()
    {
        var scene = App.GetService<CableSceneViewModel>()!;
        var rv = new PatchData();

        // nodes
        var timelineNode = new TimelineNode(scene.Timeline) { Id = 0 };
        var uniformsNode = new UniformsNode(new()
        {
            { "iTime", 0f },
            { "iResolution", new Vector2(1280, 720) }
        })
        { Id = 1 };
        var rectNode = new RectangleNode() { Id = 2 };
        var transformNode = new Transform2DNode() { Id = 3 };
        var shaderNode = new CustomShaderNode() { Id = 4 };
        var cameraNode = new Camera2DNode() { Id = 5 };
        var meshNode = new RenderableNode() { Id = 6 };
        var renderer = new RasterizerNode { IncomingData = meshNode, Id = 7 };

        // shader nodes
        var v3_1 = new ShaderVec3Node() { Id = 8 };
        var uv_xyx = new VectorVariantNode() { Id = 9 };
        var add_1 = new Add3Node() { Id = 10 };
        var floatVal = new ShaderFloatNode() { Id = 11 };
        var cos = new CosNode() { Id = 12 };
        var mul_1 = new Mul2Node() { Id = 13 };
        var add_2 = new Add2Node() { Id = 14 };
        var v3_2 = new ShaderVec3Node() { Id = 15 };
        var outNode = new OutputInstructionNode() { Id = 16 };
        var shaderBuilder = new ShaderBuilderNode() { Id = 17 };

        var uvNode = new UVNode() { Id = 18 };
        var timeNode = new TimeNode() { Id = 19 };


        // shader defaults
        v3_1.ValueEditor.ValueX = 0;
        v3_1.ValueEditor.ValueY = 2;
        v3_1.ValueEditor.ValueZ = 4;
        uv_xyx.PatternEditor.Value = "xyx";
        floatVal.ValueEditor.Value = 0.5f;
        outNode.OutputAlphaEditor.Value = 0;

        //shader connections
        uv_xyx.InputConnection = new ShaderVariableConnection(uvNode, uv_xyx, "Input");

        add_1.Input1Connection = new ShaderOperandConnection(timeNode, add_1, "Input1");
        add_1.Input2Connection = new ShaderOperandConnection(uv_xyx, add_1, "Input2");
        add_1.Input3Connection = new ShaderOperandConnection(v3_1, add_1, "Input3");

        cos.ExpressionConnection = new ShaderExpressionConnection(add_1, cos, "Expression");
        mul_1.Input1Connection = new ShaderOperandConnection(floatVal, mul_1, "Input1");
        mul_1.Input2Connection = new ShaderOperandConnection(cos, mul_1, "Input2");

        add_2.Input1Connection = new ShaderOperandConnection(floatVal, add_2, "Input1");
        add_2.Input2Connection = new ShaderOperandConnection(mul_1, add_2, "Input2");

        v3_2.ExpressionConnection = new ShaderExpressionConnection(add_2, v3_2, "Expression");
        outNode.InputVec3Connection = new ShaderVec3Connection(v3_2, outNode, "InputVec3");
        shaderBuilder.ShaderOutputConnection = new ShaderOutputConnection(outNode, shaderBuilder, "ShaderOutput");

        // default values
        //shaderNode.SetShaderFile(new FileData(@"Shaders\FractalPyramid.glsl"));
        rectNode.WidthEditor.Value = 1280;
        rectNode.HeightEditor.Value = 720;
        transformNode.ScaleEditor.ValueX = 1;
        transformNode.ScaleEditor.ValueY = 1;
        transformNode.RotationEditor.Value = 0;
        transformNode.TranslationEditor.ValueX = 0;
        transformNode.TranslationEditor.ValueY = 0;
        cameraNode.ZoomEditor.Value = 1;

        // connections

        shaderNode.ShaderBuilderConnection = new ShaderBuilderConnection(shaderBuilder, shaderNode, "ShaderBuilder");
        shaderNode.UniformsConnection = new UniformCollectionConnection(uniformsNode, shaderNode, "Uniforms");
        var itimeConn = new GenericPropertyConnection(timelineNode, uniformsNode, "iTime", "FrameTime");
        uniformsNode.AddConnectionForCollection("iTime", itimeConn);
        meshNode.ShapeConnection = new ShapeConnection(rectNode, meshNode, nameof(RenderableNode.Shape));
        meshNode.TransformConnection = new Transform2DConnection(transformNode, meshNode, nameof(RenderableNode.Transform));
        meshNode.MaterialConnection = new MaterialConnection(shaderNode, meshNode, nameof(RenderableNode.Material));
        renderer.CameraConnection = new Camera2DConnection(cameraNode, renderer, nameof(RasterizerNode.Camera));
        var meshToRenderer = new GenericConnection(meshNode, renderer);

        rv.Nodes.AddRange([timelineNode, uniformsNode, rectNode, transformNode, shaderNode, cameraNode, meshNode, renderer]);
        rv.Nodes.AddRange([v3_1, uv_xyx, add_1, floatVal, cos, mul_1, add_2, v3_2, outNode, shaderBuilder, uvNode, timeNode]);

        rv.Connections.AddRange([
            shaderNode.UniformsConnection,
            itimeConn,
            meshNode.ShapeConnection,
            meshNode.TransformConnection,
            meshNode.MaterialConnection,
            meshToRenderer,
            renderer.CameraConnection
        ]);

        rv.Connections.AddRange([
            uv_xyx.InputConnection,
            add_1.Input1Connection,
            add_1.Input2Connection,
            add_1.Input3Connection,
            cos.ExpressionConnection,
            mul_1.Input1Connection,
            mul_1.Input2Connection,
            add_2.Input1Connection,
            add_2.Input2Connection,
            v3_2.ExpressionConnection,
            outNode.InputVec3Connection,
            shaderBuilder.ShaderOutputConnection,
            shaderNode.ShaderBuilderConnection
        ]);

        rv.SelectedNodeId = 7;
        return rv;
    }
}