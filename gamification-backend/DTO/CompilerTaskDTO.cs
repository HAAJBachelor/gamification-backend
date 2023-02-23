using gamification_backend.Models;

namespace gamification_backend.DTO;

public class CompilerTaskDTO
{
    public int SessionId { get; set; }
    public string Language { get; set; }
    public List<TestCase> TestCases { get; set; }
    public string UserCode { get; set; }
}