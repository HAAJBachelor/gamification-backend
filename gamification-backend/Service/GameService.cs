using GamificationBackend.DAL;
using GamificationBackend.Models;

namespace GamificationBackend.Service;

public class GameService : IGameService
{
    private readonly IGameRepository _repo;

    public GameService(IGameRepository repo)
    {
        _repo = repo;
    }
    
    public string CreateSession(string username)
    {
        GameManager.CreateSession(username);
        return "Done";
    }

    public List<GameSession> GetSessions()
    {
        return GameManager.GetSessions();
    }
}