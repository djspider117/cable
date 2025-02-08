using Cable.App.Extensions;
using Cable.App.Models.Messages;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace Cable.App.Views.Controls;

public class NodeView : Control
{
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(NodeViewModel), typeof(NodeView), new PropertyMetadata(null));

    private static int _zIndex = 0;
    private bool _isDragging;
    private Point _initialHit;

    public DataConnector? PART_HeaderInput { get; set; }
    public DataConnector? PART_HeaderOutput { get; set; }
    public ItemsControl? PART_PropertyContainer { get; set; }

    public NodeViewModel ViewModel
    {
        get => (NodeViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    static NodeView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PART_HeaderInput = GetTemplateChild(nameof(PART_HeaderInput)) as DataConnector;
        PART_HeaderOutput = GetTemplateChild(nameof(PART_HeaderOutput)) as DataConnector;
        PART_PropertyContainer = GetTemplateChild(nameof(PART_PropertyContainer)) as ItemsControl;

        PART_HeaderOutput.ConnectionStarted += PART_HeaderOutput_ConnectionStarted;
    }

    private void PART_HeaderOutput_ConnectionStarted(object? sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send<StartNodeConnectionMessage>(new (this));
    }

    public DataConnector? GetDataConnectorForProperty(string? propertyName)
    {
        if (PART_PropertyContainer == null || propertyName == null)
            return null;

        var dataObj = ViewModel.PropertyEditors.FirstOrDefault(x => x.DisplayName == propertyName);
        var ui = PART_PropertyContainer.ItemContainerGenerator.ContainerFromItem(dataObj);
        if (PART_PropertyContainer.ItemContainerGenerator.ContainerFromItem(dataObj) is DependencyObject container)
        {
            return container.FindVisualChild<DataConnector>();
        }

        return null;
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
