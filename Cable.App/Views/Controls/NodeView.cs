using Cable.App.ViewModels.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cable.App.Views.Controls;

public class NodeView : Control
{
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(NodeViewModel), typeof(NodeView), new PropertyMetadata(null));

    private static int _zIndex = 0;
    private bool _isDragging;
    private Point _initialHit;

    public NodeViewModel ViewModel
    {
        get => (NodeViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    static NodeView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        _isDragging = true;
        _initialHit = e.GetPosition(this);

        Panel.SetZIndex(this, ++_zIndex);
       
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!_isDragging)
            return;

        var pos = e.GetPosition(Parent as IInputElement);
        var deltaX = pos.X - _initialHit.X;
        var deltaY = pos.Y - _initialHit.Y;
        ViewModel.X = deltaX;
        ViewModel.Y = deltaY;

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        _isDragging = false;

        base.OnMouseUp(e);
    }
}
