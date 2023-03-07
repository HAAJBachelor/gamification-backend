using gamification_backend.DTO;

namespace gamification_backend.Game;

public interface IStateManager
{
    public void UpdateState(int lives, int time, int points);
    public StateDTO GetState();

    public void EndSession();

    public void PauseSession();

    public void ResumeSession();

    public void StartSession();

    public bool IsRunning();

    public bool IsPaused();

    public bool IsEnded();
}