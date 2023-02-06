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

    public Dictionary<int, GameSession> GetSessions()
    {
        return _manager.GetSessions();
    }

    public TaskResult SubmitTask(string input)
    {
        _manager.CreateSession();
        return _manager.SubmitTask(input);
    }

    public List<GameTask> GenerateTaskSet()
    {
        List<GameTask> tasks = _repo.GenerateTaskSet();
        _manager.SaveTaskSet(tasks);
        return tasks;
    }

    public GameTask SelectTask(int id)
    {
        return _manager.SelectTask(id);
    }
}