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
            Rewards = gameTask.Rewards
        };
        return dto;
    }

    public static List<GameTaskDTO> GameTaskMapper(IEnumerable<GameTask> gameTasks)
    {
        return gameTasks.Select(GameTaskMapper).ToList();
    }
}