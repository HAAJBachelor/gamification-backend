using gamification_backend.DBData;
using gamification_backend.DTO;
using gamification_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace gamification_backend.DAL;

public class SessionRepository : ISessionRepository
{
    public ApplicationDbContext _db;

    public SessionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> SaveSession(SessionRecord sessionRecord)
    {
        try
        {
            _db.SessionRecords.Add(sessionRecord);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public void SaveUsername(Guid sessionId, string username)
    {
        var record = _db.SessionRecords.First(x => x.SessionId.Equals(sessionId));
        record.Username = username;
        _db.SessionRecords.Update(record);
        _db.SaveChangesAsync();
    }

    public async Task<List<SessionRecordDTO>> GetLeaderboard()
    {
        var leaderboard = await _db.SessionRecords.OrderByDescending(s => s.Score).Take(10).ToListAsync();
        var leaderboardDTO = leaderboard.Select(a => new SessionRecordDTO
            { Score = a.Score, Time = a.Time, Username = a.Username }).ToList();
        return leaderboardDTO;
    }
}