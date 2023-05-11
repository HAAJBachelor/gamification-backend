using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public interface IGameManager
{
    public GameTask? TestTask { get; set; }
    public bool CreateSession(Guid id, EventHandler<TimerDepletedEventArgs> del);
    public GameTask SelectTask(Guid sessionId, int taskId);
    public TaskResult SubmitTask(Guid sessionId, string input);
    public void SaveTaskSet(Guid sessionId, List<GameTask> tasks);
    public TestCaseResult SubmitTestCase(Guid sessionId, string input, int id);
    public TestCaseResult SubmitTestTaskTestCase(string input, int id);
    public StateDTO GetState(Guid sessionId);
    public void RemoveSession(Guid sessionId);
    public string GetStartCode(Guid sessionId, StubGenerator.Language language);
    bool IsGameSessionActive(Guid id);
    string GetTestTaskStartCode(StubGenerator.Language language);
    GameTask? GetSelectedTask(Guid getSessionId);
    public void CancelSession(Guid sessionId);

    public List<string> FinishedTasks(Guid id);

    public bool UseSkip(Guid sessionId);

    public int GetScore(Guid sessionId);

    public int GetSessionTime(Guid sessionId);
    bool HasGeneratedTaskSet(Guid sessionId);
    public List<GameTask>? GetGeneratedTaskSet(Guid sessionId);
}