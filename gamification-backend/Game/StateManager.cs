using gamification_backend.DTO;

namespace gamification_backend.Game;

public class StateManager : IStateManager
{
    private readonly Timer _timer;
    private int _lives;
    private int _points;

    public StateManager(int seconds, GameSession.EventHandler handler)
    {
        _timer = new Timer(600, handler);
        _points = 0;
        _lives = 3; // Example amount
    }

    public void UpdateState(int lives, int time, int points)
    {
        UpdateLife(lives);
        AddTime(time);
        UpdatePoints(points);
    }

    //Returns state as a State-object containing all data.
    public StateDTO GetState()
    {
        Console.WriteLine("Creating new StateDTO");
        var elapsed = _timer.StartTime - _timer.Seconds;
        return new StateDTO(_points, _lives, _timer.Seconds, elapsed);
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

    private void AddTime(int seconds)
    {
        if (seconds < 0) throw new ArgumentOutOfRangeException("Cannot award a negative amount of time");
        _timer.AddTime(seconds);
    }
}