using gamification_backend.DTO;
using gamification_backend.Models;

namespace gamification_backend.DAL;

public interface ISessionRepository
{
    // Interface for SessionRepository
    // All methods in SessionRepository must be added here.

    public Task<bool> SaveSession(SessionRecord sessionRecord);
    public void SaveUsername(Guid sessionId, string username);
    public Task<List<SessionRecordDTO>> GetLeaderboard();
}