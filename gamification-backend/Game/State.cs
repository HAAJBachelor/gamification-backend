namespace gamification_backend.Game;

public class State
{
    public State(int points, int lives, int time)
    {
        _points = points;
        _lives = lives;
        _time = time;
    }

    private int _points { get; set; }
    private int _lives { get; }

    private int _time { get; }
}