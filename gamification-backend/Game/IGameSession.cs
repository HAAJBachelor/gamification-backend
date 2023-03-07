using gamification_backend.DTO;
using gamification_backend.Models;

namespace gamification_backend.Game;

public interface IGameSession
{
    public GameTask StartNewTask(int id);
    public void SaveGeneratedTaskSet(List<GameTask> tasks);
    public TaskResult SubmitTask(string input);
    public StateDTO GetState();
}