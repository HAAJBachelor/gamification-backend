namespace gamification_backend.Models;

public class StateManager
{
    private readonly Timer _timer;
    private int _lives;
    private int _points;

    public StateManager(int seconds)
    {
        _timer = new Timer(seconds);
        _points = 0;
        _lives = 3; // Example amount
    }

    public void UpdateLife(int amount)
    {
        var newAmount = _lives + amount;
        if (newAmount < 0)
        {
            _lives = 0;
        }
        else _lives = newAmount;
    }

    public void UpdatePoints(int amount)
    {
        _points += amount;
    }

    public void AddTime(int seconds)
    {
        _timer.AddTime(seconds);
    }

    //Returns state as a State-object containing all data.
    public State GetState()
    {
        return new State(_points, _lives, _timer.GetTime());
    }
}