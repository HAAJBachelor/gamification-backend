namespace gamification_backend.Models;

public class TaskResult
{
    public TaskResult(List<TestCaseResult> testCaseResults, bool success, bool error)
    {
        TestCaseResults = testCaseResults;
        Success = success;
        CompilerError = error;
    }

    public bool Success { get; set; }

    public bool CompilerError { get; set; }
    public List<TestCaseResult> TestCaseResults { get; }
}