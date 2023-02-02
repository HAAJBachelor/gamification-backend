using gamification_backend.Models;

namespace gamification_backend.DAL;

public class GameRepository : IGameRepository
{
    public GameTask GetTask()
    {
        var task = new GameTask();
        task.Description = "Print a string to console";
        task.addSingleTestCase(new TestCase("hey", "hey"));
        return task;
    }
}