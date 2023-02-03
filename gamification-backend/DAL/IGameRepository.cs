using gamification_backend.Models;

namespace gamification_backend.DAL
{
    public interface IGameRepository
    {
        // Interface for GameRepository
        // All methods in GameRepository must be added here.

        public List<GameTask> GenerateTaskSet();
    }
}