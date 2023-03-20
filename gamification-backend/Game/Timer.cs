using gamification_backend.Utility;

namespace gamification_backend.Game;

/// <summary>
///     Keeps track of time left and time elapsed
/// </summary>
public class Timer : ITimer
{
    private readonly StateManager _manager;
    private bool _count;

    public Timer(StateManager manager, int seconds, EventHandler<EventArgsFromTimer> handler)
    {
        Seconds = seconds;
        _count = false;
        _manager = manager;
        Counter(handler);
    }

    public int Seconds { get; set; }
    public int Elapsed { get; set; }

    public void AddTime(int seconds)
    {
        if (seconds < 0) seconds = 0;
        Seconds += seconds;
    }

    private async void Counter(EventHandler<EventArgsFromTimer> handler)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync())
        {
            if (_manager.InTaskSelect()) continue;
            Seconds--;
            Elapsed++;
            if (Seconds < 0) handler.Invoke(this, new EventArgsFromTimer(Elapsed));
            if (_manager.IsEnded()) break;
        }
    }
}