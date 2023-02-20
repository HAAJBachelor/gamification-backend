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
            InputDescription = task.InputDescription,
            OutputDescription = task.OutputDescription,
            Constraints = task.Constraints,
            TestCases = task.TestCases
                .Select(taskTestCase => new TestCase(taskTestCase.TestCaseInput, taskTestCase.TestCaseOutput))
                .ToList(),
            StubCode = task.Stub,
            Category = task.Category,
            Difficulty = task.Difficulty,
            Rewards = new TaskRewards
            {
                Lives = task.Rewards.Where(reward => reward.Type == "lives").Sum(r => r.Amount),
                Points = task.Rewards.Where(reward => reward.Type == "points").Sum(r => r.Amount),
                Time = task.Rewards.Where(reward => reward.Type == "time").Sum(r => r.Amount),
            }
        };
        return gt;
    }
}