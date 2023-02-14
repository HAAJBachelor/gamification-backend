using gamification_backend.DTO;

namespace gamification_backend.Game;

public interface IStateManager
{
    public void UpdateState(int lives, int time, int points);
    public StateDTO GetState();
}