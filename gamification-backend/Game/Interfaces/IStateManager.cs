using gamification_backend.DTO;

namespace gamification_backend.Game;

public interface IStateManager
{
    public void UpdateState(int lives, int time, int points);
    public StateDTO GetState();

    public int GetTime();

    public void EndGame();

    public void SetInTaskSelect();

    public void SetInTask();

    public void StartSession();

    public bool InTask();

    public bool InTaskSelect();

    public bool IsEnded();

    public bool UseSkip();
}