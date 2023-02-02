using GamificationBackend.DAL;
using GamificationBackend.Models;

namespace GamificationBackend.Service;

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
}