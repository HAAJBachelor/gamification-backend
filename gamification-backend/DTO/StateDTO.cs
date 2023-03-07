using gamification_backend.Game;

namespace gamification_backend.DTO;

public class StateDTO
{
    public StateDTO(int points, int lives, int time, int elapsed, StateManager.RunningState runningState)
    {
        _points = points;
        _lives = lives;
        _time = time;
        _elapsed = elapsed;
        _runningState = runningState;
    }

    public int _points { get; set; }
    public int _lives { get; set; }

    public int _time { get; }

    public int _elapsed { get; set; }

    public StateManager.RunningState _runningState { get; set; }
}