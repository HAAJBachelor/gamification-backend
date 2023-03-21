using gamification_backend.Models;
using gamification_backend.Utility;
using Sanity.Linq;
using Task = gamification_backend.Sanity.Task;

namespace gamification_backend.DAL;

public class GameRepository : IGameRepository
{
    private readonly SanityClient _client;
    private readonly SanityDataContext _sanity;


    public GameRepository(IConfiguration configuration)
    {
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
        _client = new SanityClient(options);
    }

    public async Task<List<GameTask>> GenerateTaskSet()
    {
        var response = await _client.FetchAsync<List<Task>>("*[!(_id in path('drafts.**')) && _type == \"task\"]");
        var taskList = response.Result;
        var tasks = new List<GameTask>();
        Console.WriteLine($"Fetched {taskList.Count} tasks from sanity");
        var rnd = new Random();
        var randomIndexes = Enumerable.Range(0, taskList.Count).OrderBy(x => rnd.Next()).Take(3).ToArray();
        tasks.AddRange(randomIndexes.Select(index => TaskMapper.FromSanityTaskToGameTask(taskList[index])));
        return tasks;
    }

    public GameTask SelectTaskForTesting(string taskId)
    {
        var set = _sanity.DocumentSet<Task>();
        var task = set.Get(taskId);
        //FIXME: Find a better solution for this
        if (task == null)
            task = set.Get("drafts." + taskId);
        return TaskMapper.FromSanityTaskToGameTask(task);
    }
}