namespace GamificationBackend.Models;

public class TaskResult
{
    public TaskResult(List<TestCaseResult> testCaseResults, bool success)
    {
        TestCaseResults = testCaseResults;
        Success = success;
    }

    public bool Success { get; set; }
    public List<TestCaseResult> TestCaseResults { get; }
}