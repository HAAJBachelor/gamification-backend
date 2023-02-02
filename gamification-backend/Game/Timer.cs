namespace GamificationBackend.Models;

public class Timer : ITimer
{
    private int _seconds;
    private bool _count;

    public Timer(int seconds)
    {
        if (seconds < 60) seconds = 600; // Default value
        else _seconds = seconds;
        _count = false;
        Counter();
    }

    private async void Counter()
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync())
        {
            if(_count) _seconds--;
        }
    }

    public int GetTime()
    {
        return _seconds;
    }

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

    //GetFormattedTime?
}