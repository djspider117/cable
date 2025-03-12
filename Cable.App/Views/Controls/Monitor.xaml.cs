using Cable.App.Models.Data;
using Cable.Renderer.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cable.App.Views.Controls;
/// <summary>
/// Interaction logic for Monitor.xaml
/// </summary>
public partial class Monitor : UserControl
{
    public static readonly DependencyProperty NodeToPreviewProperty =
        DependencyProperty.Register(nameof(NodeToPreview), typeof(NodeView), typeof(Monitor), new PropertyMetadata(null, (s, e) => (s as Monitor)!.OnNodeToPreviewChanged(e)));

    private NodeDataBase? _nodeData;
    private CableRenderer _renderer;
    private D3DImage _d3dImage;

    public NodeView? NodeToPreview
    {
        get => (NodeView?)GetValue(NodeToPreviewProperty);
        set => SetValue(NodeToPreviewProperty, value);
    }

    public Monitor()
    {
        _renderer = new CableRenderer();
        InitializeComponent();
        _d3dImage = new D3DImage();
        ImageElement.Source = _d3dImage;

        Loaded += Monitor_Loaded;
    }

    private void Monitor_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= Monitor_Loaded;

        _renderer.InitializeSharpDX((int)ActualWidth, (int)ActualHeight, new WindowInteropHelper(Application.Current.MainWindow).Handle, _d3dImage);
        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        _renderer.Render(_nodeData!.GetRenderCommands());
    }

    private void OnNodeToPreviewChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is NodeView nv)
        {
            _nodeData = nv.ViewModel.Data;
            InvalidateVisual();
        }
    }
}
