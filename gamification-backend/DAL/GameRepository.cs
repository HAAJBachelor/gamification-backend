using gamification_backend.Models;
using gamification_backend.Service;

namespace gamification_backend.DAL;

public class GameRepository : IGameRepository
{
    private readonly TasksService _tasksService;

    public GameRepository(TasksService tasksService)
    {
        _tasksService = tasksService;
    }

    /*public GameTask GetTask()
    {
        var task = new GameTask();
        task.Description = "Print a string to console";
        task.AddSingleTestCase(new TestCase("hey", "hey"));
        return task;
    }
*/
    public List<GameTask> GenerateTaskSet()
    {
        var t = _tasksService.GetAsync();
        return t.Result;

        // Makes 3 GameTask objects and returns a List<GameTask> containing all 3 objects

        var taskset = new List<GameTask>();

        for (int i = 0; i < 3; i++)
        {
            var task = new GameTask("Description " + i, 2, 180);
            task.AddSingleTestCase(new TestCase("input" + i, "input" + i));
            taskset.Add(task);
        }

        return taskset;
    }
}