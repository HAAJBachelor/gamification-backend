using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;

namespace gamification_backend.Service;

public interface IGameService
{
    public void CreateSession(Guid id);
    public TaskResult SubmitTask(Guid sessiondId, string input);
    public TestCaseResult SubmitTestCase(Guid sessionId, string input, int index);
    public TestCaseResult SubmitTestTaskTestCase(string input, int index);
    public List<GameTaskDTO> GenerateTaskSet(Guid sessionId);
    public GameTaskDTO SelectTask(Guid sessionId, int id);
    public StateDTO GetState(Guid sessionId);
    public string GetStartCode(Guid sessionId, StubGenerator.Language language);
    string GetTestTaskStartCode(StubGenerator.Language language);
    GameTaskDTO SelectTaskForTesting(string taskId);
    void SaveUsername(Guid sessionId, string username);
    bool IsGameSessionActive(Guid sessionId);
    public void EndSession(Guid sessionId);
    GameTask? GetSelectedTask(Guid getSessionId);
}