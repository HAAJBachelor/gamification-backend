namespace gamification_backend.Models;

public interface ITimer
{
    public int GetTime();
    public void AddTime(int seconds);
    public void Start();
    public void Pause();
}