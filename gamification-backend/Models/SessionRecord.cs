namespace gamification_backend.Models;

public class SessionRecord
{
    public SessionRecord(int id, int score, int time)
    {
        Id = id;
        Score = score;
        Time = time;
    }

    public int Id { get; }
    public int Score { get; }
    public int Time { get; }
    public int Username { get; }
}