using gamification_backend.Game;
using gamification_backend.Models;

namespace gamification_backend.Service;

public interface IGameService
{
    public int CreateSession();

    public Dictionary<int, GameSession> GetSessions();

    public TaskResult SubmitTask(string input);

    public List<GameTask> GenerateTaskSet();

    public GameTask SelectTask(int id);
}