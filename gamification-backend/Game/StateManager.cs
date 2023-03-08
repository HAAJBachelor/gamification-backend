using gamification_backend.DTO;

namespace gamification_backend.Game;

public class StateManager : IStateManager
{
    public enum RunningState
    {
        Running,
        Paused,
        Ended
    }

    private readonly Timer _timer;
    private int _lives;
    private int _points;

    private RunningState _runningState;

    public StateManager(int seconds, GameSession.EventHandler handler)
    {
        _timer = new Timer(seconds, handler);
        _points = 0;
        _lives = 3; // Example amount
    }

    public void UpdateState(int lives, int time, int points)
    {
        UpdateLife(lives);
        AddTime(time);
        UpdatePoints(points);
    }

    //Returns state as a RunningState-object containing all data.
    public StateDTO GetState()
    {
        var elapsed = _timer.StartTime - _timer.Seconds;
        return new StateDTO(_points, _lives, _timer.Seconds, elapsed, _runningState);
    }

    public void EndSession()
    {
        _runningState = RunningState.Ended;
    }

    public void PauseSession()
    {
        _runningState = RunningState.Paused;
    }

    public void StartSession()
    {
        _runningState = RunningState.Running;
    }

    public void ResumeSession()
    {
        _runningState = RunningState.Running;
    }

    public bool IsRunning()
    {
        return _runningState == RunningState.Running;
    }

    public bool IsPaused()
    {
        return _runningState == RunningState.Paused;
    }

    public bool IsEnded()
    {
        return _runningState == RunningState.Ended;
    }

    public int GetTime()
    {
        return _timer.Seconds;
    }

    private void UpdateLife(int amount)
    {
        var newAmount = _lives + amount;
        if (newAmount < 0)
        {
            _lives = 0;
        }
        else _lives = newAmount;
    }

    private void UpdatePoints(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Cannot award less than 0 points.");
        _points += amount;
    }

    private void AddTime(int minutes)
    {
        if (minutes < 0) throw new ArgumentOutOfRangeException("Cannot award a negative amount of time");
        var seconds = minutes * 60;
        _timer.AddTime(seconds);
    }
}