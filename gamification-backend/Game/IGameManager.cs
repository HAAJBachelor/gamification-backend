using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public interface IGameManager
{
    public void CreateSession(string id, EventHandler<TimerDepletedEventArgs> eventHandler);
    public GameTask SelectTask(string sessionId, int taskId);
    public TaskResult SubmitTask(string sessionId, string input);
    public void SaveTaskSet(string sessionId, List<GameTask> tasks);
    public TestCaseResult SubmitTestCase(string sessionId, string input, int id);
    public StateDTO GetState(string sessionId);
    public void RemoveSession(string sessionId);
    public string GetStartCode(string sessionId, StubGenerator.Language language);
    bool IsGameSessionActive(string id);
}