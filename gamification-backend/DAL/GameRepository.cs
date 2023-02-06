using gamification_backend.Models;

namespace gamification_backend.DAL;

public class GameRepository : IGameRepository
{
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