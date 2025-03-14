namespace Cable.Core;

public interface ILayoutable
{
    double ActualWidth { get; }
    double ActualHeight { get; }

    double X { get; set; }
    double Y { get; set; }
}
