using Cable.Data.Types;
using System.Windows.Threading;

namespace Cable.App.Models.Data;

public interface IPlayable
{
    void Start();
    void Stop();
    void Pause();
}

public class CableTimeline : IPlayable, ICableDataType
{
    private readonly DispatcherTimer _timer;

    public uint FrameIndex { get; set; }
    public uint FrameRate { get; set; } = 60;
    public double SecondsFromStart => (double)FrameIndex / FrameRate;

    public CableTimeline(bool autostart = true)
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.0d / FrameRate) };
        _timer.Tick += Timer_Tick;

        if (autostart)
            Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        FrameIndex = (FrameIndex + 1 ) % 500; //TODO: temp for testing
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        FrameIndex = 0;
    }

    public void Pause()
    {
        _timer.Stop();
    }
}
