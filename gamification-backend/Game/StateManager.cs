using System.Reflection.Metadata;

namespace GamificationBackend.Models;

public class StateManager
{
    private readonly GameSession _session;
    private Timer _timer;
    private int _points;
    private int _lives;

    public StateManager(GameSession s)
    {
        _session = s;
        _timer = new Timer();
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

    public void addTime(int seconds)
    {
        //_timer.AddTime(int seconds);
    }

    //Returns state as a State-object containing all data.
    public State GetState()
    {
        return new State(_points, _lives, _timer);
    }

}