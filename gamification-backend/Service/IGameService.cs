using gamification_backend.Game;
using gamification_backend.Models;

namespace gamification_backend.Service;

public interface IGameService
{
    public string CreateSession(string username);

    public Dictionary<int, GameSession> GetSessions();

    public TaskResult SubmitTask(string input);

    public void GetTask();
}