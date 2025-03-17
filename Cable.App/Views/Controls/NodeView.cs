using Cable.App.Extensions;
using Cable.App.Models.Messages;
using Cable.App.ViewModels.Data;
using Cable.App.ViewModels.Data.PropertyEditors;
using Cable.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Cable.App.Views.Controls;

public class NodeView : Control, ILayoutable
{
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(NodeViewModel), typeof(NodeView), new PropertyMetadata(null, (s, e) => (s as NodeView)!.OnViewModelChanged(e)));

    private static int _zIndex = 0;
    private bool _isDragging;
    private bool _wasDragged;
    private Point _initialHit;

    public DataConnector? PART_HeaderInput { get; set; }
    public DataConnector? PART_HeaderOutput { get; set; }
    public ItemsControl? PART_PropertyContainer { get; set; }

    public NodeViewModel ViewModel
    {
        get => (NodeViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public double X
    {
        get => ViewModel.X;
        set => ViewModel.X = value;
    }

    public double Y
    {
        get => ViewModel.Y;
        set => ViewModel.Y = value;
    }


    static NodeView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
    }

    private void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
    {
        var vm = e.NewValue as NodeViewModel;
        vm!.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var vm = sender as NodeViewModel;
        if (e.PropertyName == nameof(NodeViewModel.IsSelected))
        {
            if (vm!.IsSelected)
            {
                BorderThickness = new Thickness(2);
                BorderBrush = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                BorderThickness = new Thickness(0);
                BorderBrush = null;
            }
        }
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
        _wasDragged = false;
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
        _wasDragged = true;

        base.OnMouseMove(e);
    }
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        _isDragging = false;
        e.Handled = _wasDragged;
        base.OnMouseUp(e);
    }
}
