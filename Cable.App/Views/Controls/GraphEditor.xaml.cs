using Cable.App.Extensions;
using Cable.App.Models.Data.Connections;
using Cable.App.Models.Messages;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Data.Types;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cable.App.Views.Controls;

public partial class GraphEditor : UserControl, INodeViewResolver
{
    public static readonly DependencyProperty SelectedNodeViewProperty =
        DependencyProperty.Register(nameof(SelectedNodeView), typeof(NodeView), typeof(GraphEditor), new PropertyMetadata(null));


    private readonly Dictionary<NodeViewModel, NodeView> _nodeMapping = [];
    private NodeConnectionView? _pendingConnection;

    public NodeView? SelectedNodeView
    {
        get => (NodeView)GetValue(SelectedNodeViewProperty);
        set => SetValue(SelectedNodeViewProperty, value);
    }

    public GraphEditor()
    {
        InitializeComponent();

        BuildDemoGraph2();

        WeakReferenceMessenger.Default.Register<StartNodeConnectionMessage>(this, OnStartNodeConnectionMessage);
        Application.Current.MainWindow.MouseUp += MainWindow_MouseUp;
    }

    private void BuildDemoGraph()
    {
        var numberNode = new FloatNode(10.2f);
        var vectorNode = new Float2Node(new Vector2(5, 12));
        var transformNode = new Transform2DNode();

        var c1 = new FloatConnection(numberNode, transformNode, "Rotation");
        var c2 = new Float2Connection(vectorNode, transformNode, "Translation");

        transformNode.RotationConnection = c1;
        transformNode.TranslationConnection = c2;

        var v1 = new NodeView() { ViewModel = new NodeViewModel(numberNode) };
        var v2 = new NodeView() { ViewModel = new NodeViewModel(vectorNode) };
        var v3 = new NodeView() { ViewModel = new NodeViewModel(transformNode) };

        pnlNodeContainer.Children.Add(v1);
        pnlNodeContainer.Children.Add(v2);
        pnlNodeContainer.Children.Add(v3);

        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v1, v3, c1));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v2, v3, c2));
    }

    private void BuildDemoGraph2()
    {
        var scene = App.GetService<CableSceneViewModel>()!;

        var timelineNode = new TimelineNode(scene.Timeline);

        var rectNode = new RectangleNode();
        var transformNode = new Transform2DNode();
        var gradientNode = new GradienteNode();

        rectNode.WidthConnection = new UIntToFloatConnection(timelineNode, rectNode, "Width", "FrameIndex");

        //rectNode.WidthEditor.Value = 100;
        rectNode.HeightEditor.Value = 50;

        transformNode.ScaleEditor.ValueX = 1;
        transformNode.ScaleEditor.ValueY = 1;
        transformNode.RotationEditor.Value = 45;
        transformNode.TranslationEditor.ValueX = 200;
        transformNode.TranslationEditor.ValueY = 200;


        var meshNode = new RenderableNode();
        meshNode.ShapeConnection = new ShapeConnection(rectNode, meshNode, nameof(RenderableNode.Shape));
        meshNode.TransformConnection = new Transform2DConnection(transformNode, meshNode, nameof(RenderableNode.Transform));
        meshNode.MaterialConnection = new MaterialConnection(gradientNode, meshNode, nameof(RenderableNode.Material));

        var cameraNode = new Camera2DNode();
        cameraNode.ZoomEditor.Value = 1;

        var renderer = new RasterizerNode { IncomingData = meshNode };
        renderer.CameraConnection = new Camera2DConnection(cameraNode, renderer, nameof(RasterizerNode.Camera));

        // views

        var v0 = new NodeView() { ViewModel = new NodeViewModel(timelineNode) };
        var v1 = new NodeView() { ViewModel = new NodeViewModel(rectNode) };
        var v2 = new NodeView() { ViewModel = new NodeViewModel(transformNode) };
        var v3 = new NodeView() { ViewModel = new NodeViewModel(gradientNode) };
        var v4 = new NodeView() { ViewModel = new NodeViewModel(meshNode) };
        var v5 = new NodeView() { ViewModel = new NodeViewModel(cameraNode) };
        var v6 = new NodeView() { ViewModel = new NodeViewModel(renderer) };

        pnlNodeContainer.Children.Add(v0);
        pnlNodeContainer.Children.Add(v1);
        pnlNodeContainer.Children.Add(v2);
        pnlNodeContainer.Children.Add(v3);
        pnlNodeContainer.Children.Add(v4);
        pnlNodeContainer.Children.Add(v5);
        pnlNodeContainer.Children.Add(v6);


        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v0, v1, rectNode.WidthConnection));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v1, v4, meshNode.ShapeConnection));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v2, v4, meshNode.TransformConnection));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v3, v4, meshNode.MaterialConnection));

        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v4, v6, null));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v5, v6, renderer.CameraConnection));
        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v5, v6, renderer.CameraConnection));

        SelectedNodeView = v6;
        monitor.NodeToPreview = v6;
    }

    private void BuildDemoGraph3()
    {
        var scene = App.GetService<CableSceneViewModel>()!;

        var timelineNode = new TimelineNode(scene.Timeline);
        var floatNode = new FloatNode(0);

        var c1 = new UIntToFloatConnection(timelineNode, floatNode, "Value", "FrameIndex");


        var v1 = new NodeView() { ViewModel = new NodeViewModel(timelineNode) };
        var v2 = new NodeView() { ViewModel = new NodeViewModel(floatNode) };

        pnlNodeContainer.Children.Add(v1);
        pnlNodeContainer.Children.Add(v2);

        pnlConnectionsContainer.Children.Add(new NodeConnectionView(v1, v2, c1));
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


        if (_pendingConnection?.Destination == null)
            pnlConnectionsContainer.Children.Remove(_pendingConnection);
    }

    private void OnStartNodeConnectionMessage(object recipient, StartNodeConnectionMessage message)
    {
        _pendingConnection = new NodeConnectionView(message.SourceView, null, null!);
        pnlConnectionsContainer.Children.Add(_pendingConnection);
    }


    public NodeView? GetViewFromViewModel(NodeViewModel? vm)
    {
        throw new NotImplementedException();
    }
}