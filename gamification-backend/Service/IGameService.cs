using GamificationBackend.Models;

namespace GamificationBackend.Service;

public interface IGameService
{
    public string CreateSession(string username);
    
    public List<GameSession> GetSessions();
}