using gamification_backend.Models;
using gamification_backend.Service;
using gamification_backend.Utility;
using Sanity.Linq;
using Task = gamification_backend.Sanity.Task;

namespace gamification_backend.DAL;

public class GameRepository : IGameRepository
{
    private readonly SanityDataContext _sanity;
    private readonly TasksService _tasksService;


    public GameRepository(TasksService tasksService, IConfiguration configuration)
    {
        if (_tasksService != null)
            return;
        _tasksService = tasksService;
        if (_sanity != null)
            return;
        var token = configuration["CMS:Token"];
        var projectId = configuration["CMS:ProjectID"];
        var options = new SanityOptions
        {
            ProjectId = projectId,
            Dataset = "production",
            Token = token,
            UseCdn = false,
            ApiVersion = "v1"
        };
        _sanity = new SanityDataContext(options);
    }

    /*public GameTask GetTask()
    {
        var task = new GameTask();
        task.Description = "Print a string to console";
        task.AddSingleTestCase(new TestCase("hey", "hey"));
        return task;
    }
*/
    public async Task<List<GameTask>> GenerateTaskSet()
    {
        var set = _sanity.DocumentSet<Task>();
        var taskList = await set.ToListAsync();
        var tasks = new List<GameTask>();
        Console.WriteLine($"Fetched {taskList.Count} tasks from sanity");
        taskList.ForEach(t => tasks.Add(TaskMapper.FromSanityTaskToGameTask(t)));
        return tasks;
    }

    public async void SaveSession(SessionRecord sessionRecord)
    {
        Console.WriteLine("Saving session from repo");
        await _tasksService.CreateAsync(sessionRecord);
        var s = await _tasksService.GetAsync(sessionRecord.Id);
        Console.WriteLine(s.Time);
    }
}