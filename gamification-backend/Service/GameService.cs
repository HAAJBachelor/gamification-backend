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

    public string CreateSession(string username)
    {
        _manager.CreateSession(username);
        return "Done";
    }

    public Dictionary<int, GameSession> GetSessions()
    {
        return _manager.GetSessions();
    }

    public TaskResult SubmitTask(string input)
    {
        GetTask();
        _manager.CreateSession("Ole");
        return _manager.SubmitTask(input);
    }

    public void GetTask()
    {
        _manager.AddTask(_repo.GetTask());
    }

    public List<GameTask> GenerateTaskSet()
    {
        return _repo.GenerateTaskSet();
    }
}