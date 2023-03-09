using gamification_backend.Game;

namespace gamification_backend.DTO;

public class StateDTO
{
    public StateDTO(int points, int lives, StateManager.RunningState runningState)
    {
        _points = points;
        _lives = lives;
        _runningState = runningState;
    }

    public int _points { get; set; }
    public int _lives { get; set; }
    public StateManager.RunningState _runningState { get; set; }
}