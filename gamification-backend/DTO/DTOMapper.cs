using gamification_backend.Models;

namespace gamification_backend.DTO;

public static class DTOMapper
{
    public static GameTaskDTO GameTaskMapper(GameTask gameTask)
    {
        GameTaskDTO dto = new()
        {
            Description = gameTask.Description,
            TaskId = gameTask.TaskId,
            StartCode = gameTask.StartCode,
            TestCases = gameTask.TestCases,
            Rewards = gameTask.Rewards,
            Difficulty = gameTask.Difficulty
        };
        return dto;
    }

    public static List<GameTaskDTO> GameTaskMapper(IEnumerable<GameTask> gameTasks)
    {
        return gameTasks.Select(GameTaskMapper).ToList();
    }

    public static CompilerTaskDTO FromGameTaskToCompilerTask(GameTask gameTask, int testcaseIndex)
    {
        List<TestCase> singleCase = new(1);
        if (testcaseIndex != -1)
            singleCase.Add(gameTask.SingleTestCase(testcaseIndex));
        CompilerTaskDTO dto = new()
        {
            SessionId = gameTask.SessionId,
            Language = gameTask.Language,
            TestCases = testcaseIndex == -1 ? gameTask.TestCases : singleCase,
            UserCode = gameTask.UserCode
        };
        return dto;
    }
}