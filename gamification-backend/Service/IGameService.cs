using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;

namespace gamification_backend.Service;

public interface IGameService
{
    public int CreateSession();
    public TaskResult SubmitTask(int sessiondId, string input);
    public TestCaseResult SubmitTestCase(int sessionId, string input, int index);
    public List<GameTaskDTO> GenerateTaskSet(int sessionId);
    public GameTaskDTO SelectTask(int sessionId, int id);
    public StateDTO GetState(int sessionId);
    public string GetStartCode(int sessionId, StubGenerator.Language language);
    void SaveUsername(int sessionId, string username);
    bool IsGameSessionActive(int sessionId);
}