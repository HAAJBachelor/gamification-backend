namespace gamification_backend.Models;

public class SessionRecord
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int Time { get; set; }
    public string? Username { get; set; }
}