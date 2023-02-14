namespace gamification_backend.Game;

public interface ITimer
{
    public void AddTime(int seconds);
    public void Start();
    public void Pause();
}