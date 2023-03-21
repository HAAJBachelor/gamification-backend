using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;

namespace gamification_backend.Game;

public interface IGameSession
{
    public StateManager StateManager { get; }
    public GameTask StartNewTask(int id);
    public void SaveGeneratedTaskSet(List<GameTask> tasks);
    public TaskResult SubmitTask(string input);
    public StateDTO GetState();
    public List<string> FinishedTasks();
    public TestCaseResult SubmitTestCase(string input, int id);
    public GameTask? GetCurrentTask();
    public void Cancel();
}