using gamification_backend.Models;
using Task = gamification_backend.Sanity.Task;

namespace gamification_backend.Utility;

public static class TaskMapper
{
    public static GameTask FromSanityTaskToGameTask(Task task)
    {
        var gt = new GameTask
        {
            TaskId = 0,
            Description = task.Description,
            TestCases = task.TestCases
                .Select(taskTestCase => new TestCase(taskTestCase.TestCaseInput, taskTestCase.ValidatorOutput))
                .ToList(),
            StubCode = task.Stub,
        };
        return gt;
    }
}