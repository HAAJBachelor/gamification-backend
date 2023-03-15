using gamification_backend.Models;

namespace gamification_backend.DTO;

public static class DTOMapper
{
    public static GameTaskDTO GameTaskMapper(GameTask gameTask)
    {
        GameTaskDTO dto = new()
        {
            Title = gameTask.Title,
            Description = gameTask.Description,
            InputDescription = gameTask.InputDescription,
            OutputDescription = gameTask.OutputDescription,
            Constraints = gameTask.Constraints,
            TaskId = gameTask.TaskId,
            StartCode = gameTask.StartCode,
            TestCases = gameTask.TestCases,
            Rewards = gameTask.Rewards,
            Difficulty = gameTask.Difficulty,
            Category = gameTask.Category
        };
        return dto;
    }

    public static List<GameTaskDTO> GameTaskMapper(IEnumerable<GameTask> gameTasks)
    {
        return gameTasks.Select(GameTaskMapper).ToList();
    }

    public static CompilerTaskDTO FromGameTaskToCompilerTask(GameTask gameTask, int testcaseIndex,
        bool validator = false)
    {
        if (validator)
            return new CompilerTaskDTO
            {
                SessionId = gameTask.SessionId,
                Language = gameTask.Language,
                TestCases = gameTask.ValidatorCases,
                UserCode = gameTask.UserCode
            };
        List<TestCase>? singleCase = null;
        if (testcaseIndex != -1) singleCase = new List<TestCase>(1) { gameTask.SingleTestCase(testcaseIndex) };

        return new CompilerTaskDTO
        {
            SessionId = gameTask.SessionId,
            Language = gameTask.Language,
            TestCases = singleCase ?? gameTask.TestCases,
            UserCode = gameTask.UserCode
        };
    }
}