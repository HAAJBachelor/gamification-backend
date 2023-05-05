using gamification_backend.Models;
using Task = gamification_backend.Sanity.Task;

namespace gamification_backend.Utility;

public static class TaskMapper
{
    public static GameTask FromSanityTaskToGameTask(Task task)
    {
        var gt = new GameTask
        {
            Id = task._id,
            Title = task.Title,
            Description = task.Description,
            InputDescription = task.InputDescription,
            OutputDescription = task.OutputDescription,
            Constraints = task.Constraints,
            TestCases = task.TestCases
                .Select(taskTestCase => new TestCase(taskTestCase.TestCaseInput, taskTestCase.TestCaseOutput))
                .ToList(),
            ValidatorCases = task.TestCases
                .Select(taskTestCase => new TestCase(taskTestCase.ValidatorInput, taskTestCase.ValidatorOutput))
                .ToList(),
            StubCode = task.Stub,
            Category = task.Category.Select(category => category.Name).ToArray(),
            Difficulty = task.Difficulty,
            Rewards = new TaskRewards
            {
                Lives = task.Rewards.Where(reward => reward.Type == "lives").Sum(r => r.Amount),
                Time = task.Rewards.Where(reward => reward.Type == "time").Sum(r => r.Amount),
                Score = task.Score
            },
        };
        return gt;
    }
}