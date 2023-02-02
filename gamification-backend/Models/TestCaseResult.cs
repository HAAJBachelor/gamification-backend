namespace gamification_backend.Models;

public class TestCaseResult
{
    public bool Success { get; set; } = false;
    public bool Error { get; set; } = false;
    public string? Description { get; set; }
}