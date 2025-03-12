using Cable.App.Models.Data;
using Cable.Renderer;
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

    private CableRenderer? _nativeRenderer;
    private NodeDataBase? _nodeData;
    private DX11Host _dxHost;

    public NodeView? NodeToPreview
    {
        get => (NodeView?)GetValue(NodeToPreviewProperty);
        set => SetValue(NodeToPreviewProperty, value);
    }

    public Monitor()
    {
        InitializeComponent();

        Loaded += Monitor_Loaded;
    }

    private async void Monitor_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= Monitor_Loaded;
        await Task.Delay(100);

        _dxHost = new DX11Host((int)rect.ActualWidth, (int)rect.ActualHeight);
        DxPresenter.Content = _dxHost;
        await Task.Delay(100);
        _nativeRenderer = new CableRenderer((uint)rect.ActualWidth, (uint)rect.ActualHeight, _dxHost.GetHandle());
        CompositionTarget.Rendering += CompositionTarget_Rendering;

    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        if (_nodeData == null)
            return;

        _nativeRenderer?.Render(_nodeData.GetRenderCommands());
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
