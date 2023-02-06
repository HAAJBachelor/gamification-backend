using gamification_backend.Models;

namespace gamification_backend.Service;

public interface IGameService
{
    public int CreateSession();

    public TaskResult SubmitTask(int sessiondId, string input);

    public List<GameTask> GenerateTaskSet(int sessionId);

    public GameTask SelectTask(int sessionId, int id);
}