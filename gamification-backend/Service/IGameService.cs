using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;

namespace gamification_backend.Service;

public interface IGameService
{
    public void CreateSession(string id);
    public TaskResult SubmitTask(string sessiondId, string input);
    public TestCaseResult SubmitTestCase(string sessionId, string input, int index);
    public List<GameTaskDTO> GenerateTaskSet(string sessionId);
    public GameTaskDTO SelectTask(string sessionId, int id);
    public StateDTO GetState(string sessionId);
    public string GetStartCode(string sessionId, StubGenerator.Language language);
    void SaveUsername(string sessionId, string username);
    bool IsGameSessionActive(string sessionId);
}