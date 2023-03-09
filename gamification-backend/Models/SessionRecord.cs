using System.ComponentModel.DataAnnotations;

namespace gamification_backend.Models;

public class SessionRecord
{
    [Key]
    public int Id { get; set; }
    public int Score { get; set; }
    public int Time { get; set; }
    [Required]
    public int SessionId { get; set; }
    public string? Username { get; set; }
}