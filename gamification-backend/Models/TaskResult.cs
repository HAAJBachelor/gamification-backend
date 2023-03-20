namespace gamification_backend.Models;

public class TaskResult
{
    public TaskResult(List<TestCaseResult> testCaseResults, bool success, bool error, string compilerErrorMessage = "")
    {
        TestCaseResults = testCaseResults;
        Success = success;
        CompilerError = error;
        CompilerErrorMessage = compilerErrorMessage;
    }

    public bool Success { get; set; }

    public bool CompilerError { get; set; }

    public string CompilerErrorMessage { get; set; }
    public List<TestCaseResult> TestCaseResults { get; }
}