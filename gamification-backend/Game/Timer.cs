namespace gamification_backend.Game;

/// <summary>
///     Keeps track of time left and time elapsed
/// </summary>
public class Timer : ITimer
{
    private bool _count;

    public Timer(int seconds, GameSession.EventHandler handler)
    {
        StartTime = seconds;
        Seconds = StartTime;
        _count = false;
        Counter(handler);
    }

    public int Seconds { get; set; }
    public int StartTime { get; set; }

    public void AddTime(int seconds)
    {
        if (seconds < 0) seconds = 0;
        Seconds += seconds;
    }

    public void Start()
    {
        _count = true;
    }

    public void Pause()
    {
        _count = false;
    }

    private async void Counter(GameSession.EventHandler handler)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        Start();
        while (await timer.WaitForNextTickAsync())
        {
            if (!_count) continue;
            Seconds--;
            if (Seconds > 0) continue;
            Pause();
            handler.Invoke(this, EventArgs.Empty);
        }
    }

    //GetFormattedTime?
}