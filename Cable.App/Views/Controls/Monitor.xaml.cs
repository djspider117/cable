using Cable.App.Models.Data;
using Cable.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
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
public unsafe partial class Monitor : UserControl
{
    public static readonly DependencyProperty NodeToPreviewProperty =
        DependencyProperty.Register(nameof(NodeToPreview), typeof(NodeView), typeof(Monitor), new PropertyMetadata(null, (s, e) => (s as Monitor)!.OnNodeToPreviewChanged(e)));

    private CableRenderer? _nativeRenderer;
    private D3DImage _d3dImage = new();
    private void* _sharedHandle;
    private NodeDataBase? _nodeData;

    public NodeView? NodeToPreview
    {
        get => (NodeView?)GetValue(NodeToPreviewProperty);
        set => SetValue(NodeToPreviewProperty, value);
    }

    public Monitor()
    {
        InitializeComponent();
        DxImage.Source = _d3dImage;

        Loaded += Monitor_Loaded;
    }

    private void Monitor_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= Monitor_Loaded;
        _nativeRenderer = new CableRenderer((uint)ActualWidth, (uint)ActualHeight);
        _sharedHandle = _nativeRenderer.GetSharedHandle();
        CompositionTarget.Rendering += CompositionTarget_Rendering;

    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        if (_sharedHandle == null || _nodeData == null)
            return;

        _nativeRenderer?.Render(_nodeData.GetRenderCommands());

        // Lock the D3DImage
        _d3dImage.Lock();

        // Set the back buffer
        _d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (nint)_sharedHandle);

        // Unlock the D3DImage
        _d3dImage.Unlock();
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
