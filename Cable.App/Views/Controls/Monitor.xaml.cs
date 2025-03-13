using Cable.App.Models.Data;
using Cable.Renderer;
using SkiaSharp;
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
    private SkiaRenderer _renderer;

    public NodeView? NodeToPreview
    {
        get => (NodeView?)GetValue(NodeToPreviewProperty);
        set => SetValue(NodeToPreviewProperty, value);
    }

    public Monitor()
    {
        InitializeComponent();

        _renderer = new SkiaRenderer();
        Loaded += Monitor_Loaded;
    }

    private void Monitor_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= Monitor_Loaded;

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
       SkiaElement.InvalidateVisual();
    }

    private void OnNodeToPreviewChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is NodeView nv)
        {
            _nodeData = nv.ViewModel.Data;
        }
    }

    private void SkiaElement_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        _renderer.Render(e, _nodeData!.GetRenderCommands());
    }
}
