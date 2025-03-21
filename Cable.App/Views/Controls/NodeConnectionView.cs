﻿using Cable.App.Models.Data.Connections;
using Cable.App.ViewModels.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cable.App.Views.Controls;

public class NodeConnectionView : Control
{
    private readonly INodeViewResolver? _nodeViewResolver;
    private readonly ConnectionViewModel _connectionViewModel;

    private IConnection? _connection;
    private NodeView _source;
    private NodeView? _destination;

    public NodeView Source { get => _source; set => _source = value; }
    public IConnection? Connection { get => _connection; set => _connection = value; }
    public ConnectionViewModel ViewModel => _connectionViewModel;

    public NodeView? Destination
    {
        get => _destination;
        set
        {
            if (_destination != null)
                _destination.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            _destination = value;

            if (_destination != null)
                _destination.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    public NodeConnectionView(ConnectionViewModel connVm, INodeViewResolver resolver)
    {
        _nodeViewResolver = resolver;
        _connectionViewModel = connVm;

        _source = _nodeViewResolver.GetViewFromViewModel(connVm.SourceNode) ?? throw new InvalidOperationException("Source node must not be null");
        _destination = _nodeViewResolver.GetViewFromViewModel(connVm.TargetNode);
        _connection = connVm.Connection;

        AttachHandlers();
    }

    public NodeConnectionView(NodeView source, NodeView? dest, IConnection? connection)
    {
        _source = source;
        Destination = dest;
        _connection = connection;

        _connectionViewModel = new ConnectionViewModel(source.ViewModel, dest?.ViewModel, connection);

        AttachHandlers();
    }

    private void AttachHandlers()
    {
        Source.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        if (Destination != null)
            Destination.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        IsHitTestVisible = false;

        Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
    }

    private void MainWindow_MouseMove(object sender, MouseEventArgs e)
    {
        if (Destination == null)
            InvalidateVisual();
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NodeViewModel.X) ||
            e.PropertyName == nameof(NodeViewModel.Y))
        {
            InvalidateVisual();
        }
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        var srcOffset = Source.PART_HeaderOutput?.TransformToVisual(Source).Transform(new());
        if (srcOffset == null)
            return;

        Point destOffset;
        Point localSrcOffset;
        double internalOffsetX = 0;
        double internalOffsetY = 0;

        //double srcInternalOffsetX = 0;
        //double srcInternalOffsetY = 0;
        if (Destination == null)
        {
            var pos = Application.Current.MainWindow.PointToScreen(Mouse.GetPosition(Application.Current.MainWindow));
            destOffset = (Parent as Panel)!.PointFromScreen(pos);
        }
        else
        {
            if (_connection == null || _connection is GenericConnection)
            {
                destOffset.X = Destination.ViewModel.X + (Destination.PART_HeaderInput!.ActualWidth / 2);
                destOffset.Y = Destination.ViewModel.Y + (Destination.PART_HeaderInput!.ActualHeight / 2);
            }
            else
            {
                if (_connection.PropertyName != null)
                {
                    destOffset = Destination.GetDataConnectorForProperty(_connection.PropertyName)?
                        .TransformToVisual(Destination)
                        .Transform(new()) ?? new Point();

                    destOffset.X += Destination.ViewModel.X;
                    destOffset.Y += Destination.ViewModel.Y;

                    internalOffsetX = Destination.PART_HeaderInput!.ActualWidth / 2;
                    internalOffsetY = Destination.PART_HeaderInput!.ActualHeight / 2;
                }

                if (_connection.SourcePropertyName != null)
                {
                    localSrcOffset = Source.GetDataConnectorForProperty(_connection.SourcePropertyName)?
                        .TransformToVisual(Source)
                        .Transform(new()) ?? new Point();

                    localSrcOffset.X += Source.ViewModel.X;
                    localSrcOffset.Y += Source.ViewModel.Y;
                }
            }
        }

        var srcX = _connection?.SourcePropertyName != null
            ? localSrcOffset.X + (Source.PART_HeaderOutput!.ActualWidth / 2)
            : Source.ViewModel.X + srcOffset.Value.X + (Source.PART_HeaderOutput!.ActualWidth / 2);
        var srcY = _connection?.SourcePropertyName != null
            ? localSrcOffset.Y + (Source.PART_HeaderOutput!.ActualHeight / 2)
            : Source.ViewModel.Y + srcOffset.Value.Y + (Source.PART_HeaderOutput!.ActualHeight / 2);

        var destX = destOffset.X + internalOffsetX;
        var destY = destOffset.Y + internalOffsetY;

        var start = new Point(srcX, srcY);
        var end = new Point(destX, destY);
        var control1 = new Point(srcX + ((destX - srcX) / 3), srcY);
        var control2 = new Point(destX - ((destX - srcX) / 3), destY);

        var geometry = new StreamGeometry();
        using (StreamGeometryContext ctx = geometry.Open())
        {
            ctx.BeginFigure(start, false, false);
            ctx.BezierTo(control1, control2, end, true, true);
        }

        geometry.Freeze();

        dc.DrawGeometry(null, new Pen(Brushes.Gray, 2), geometry);
    }
}