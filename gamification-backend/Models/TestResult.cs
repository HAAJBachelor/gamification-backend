namespace GamificationBackend.Models;

public class TestResult
{
    public bool Success { get; set; } = false;
    public bool Error { get; set; } = false;
    public string? Description { get; set; }
}