using GamificationBackend.Models;

namespace GamificationBackend.Service;

public interface IGameService
{
    public string CreateSession(string username);
    
    public Dictionary<int, GameSession> GetSessions();
}