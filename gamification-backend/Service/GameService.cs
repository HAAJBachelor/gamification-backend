using gamification_backend.DAL;
using gamification_backend.Game;
using gamification_backend.Models;

namespace gamification_backend.Service;

public class GameService : IGameService
{
    private readonly IGameRepository _repo;
    private GameManager _manager;

    public GameService(IGameRepository repo)
    {
        _repo = repo;
        _manager = GameManager.The();
    }

    public int CreateSession()
    {
        return _manager.CreateSession();
    }

    public TaskResult SubmitTask(int sessionId, string input)
    {
        return _manager.SubmitTask(sessionId, input);
    }

    public List<GameTask> GenerateTaskSet(int sessionId)
    {
        List<GameTask> tasks = _repo.GenerateTaskSet();
        _manager.SaveTaskSet(sessionId, tasks);
        return tasks;
    }

    public GameTask SelectTask(int sessionId, int taskId)
    {
        return _manager.SelectTask(sessionId, taskId);
    }
}