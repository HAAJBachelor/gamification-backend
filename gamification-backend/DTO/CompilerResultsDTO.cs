namespace gamification_backend.DTO;

public class CompilerResultsDTO
{
    public bool Error { get; set; }
    public string Error_message { get; set; }
    public List<string> Results { get; set; } = new();
}