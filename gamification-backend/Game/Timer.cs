namespace gamification_backend.Game;

public class Timer : ITimer
{
    private bool _count;

    public Timer(int seconds, GameSession.EventHandler handler)
    {
        if (seconds < 60) _seconds = 600; // Default value
        else _seconds = seconds;
        _count = false;
        Counter(handler);
    }

    public int _seconds { get; set; }
    public int _timeElapsed { get; set; }

    public void AddTime(int seconds)
    {
        if (seconds < 0) seconds = 0;
        _seconds += seconds;
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
            _seconds--;
            _timeElapsed++;
            if (_seconds <= 0)
            {
                handler(this, EventArgs.Empty);
                Pause();
            }
        }
    }

    //GetFormattedTime?
}