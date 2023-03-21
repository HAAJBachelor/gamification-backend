using gamification_backend.Models;
using Task = gamification_backend.Sanity.Task;

namespace gamification_backend.DAL
{
    public interface IGameRepository
    {
        // Interface for GameRepository
        // All methods in GameRepository must be added here.

        public Task<List<Task>> GenerateTaskSet();
        GameTask SelectTaskForTesting(string taskId);
    }
}