using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public interface IGameManager
{
    public int CreateSession(EventHandler<TimerDepletedEventArgs> del);
    public GameTask SelectTask(int sessionId, int taskId);
    public TaskResult SubmitTask(int sessionId, string input);
    public void SaveTaskSet(int sessionId, List<GameTask> tasks);
    public StateDTO GetState(int sessionId);
    public void RemoveSession(int sessionId);
}