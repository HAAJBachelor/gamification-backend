using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public class GameManager : IGameManager
{
    private static GameManager? _instance;
    private readonly Dictionary<Guid, GameSession> _sessions;

    private GameManager()
    {
        _sessions = new Dictionary<Guid, GameSession>();
    }

    public void CreateSession(Guid id, EventHandler<TimerDepletedEventArgs> eventHandler)
    {
        var session = new GameSession(id, 30, eventHandler);
        _sessions.Add(id, session);
        Console.WriteLine("Creating new session with id {0}, total: {1}", id, _sessions.Count);
    }

    public GameTask SelectTask(Guid sessionId, int taskId)
    {
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].StartNewTask(taskId);
        throw new ArgumentException("Invalid session Id. GameManager.SelectTask");
    }

    public TaskResult SubmitTask(Guid sessionId, string input)
    {
        if (!_sessions.ContainsKey(sessionId)) throw new ArgumentException("Invalid session Id");
        var session = _sessions[sessionId];
        return session.SubmitTask(input);
    }

    public TestCaseResult SubmitTestCase(Guid sessionId, string input, int id)
    {
        return _sessions[sessionId].SubmitTestCase(input, id);
    }

    public void SaveTaskSet(Guid sessionId, List<GameTask> tasks)
    {
        if (_sessions.ContainsKey(sessionId))
            _sessions[sessionId].SaveGeneratedTaskSet(tasks);
        else throw new ArgumentException("Invalid session Id " + sessionId);
    }

    public StateDTO GetState(Guid sessionId)
    {
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].GetState();
        throw new ArgumentException("Invalid session Id");
    }

    public void RemoveSession(Guid sessionId)
    {
        _sessions.Remove(sessionId);
    }

    public string GetStartCode(Guid sessionId, StubGenerator.Language language)
    {
        var session = _sessions[sessionId];
        var currentTask = session.GetCurrentTask();
        if (currentTask == null)
            return "No current task";
        if (currentTask.Language == language.ToString())
            return currentTask.StartCode;
        var code = StubService.GenerateCode(currentTask.StubCode, language);
        currentTask.StartCode = code;
        currentTask.Language = language.ToString().ToLower();
        return code;
    }

    public bool IsGameSessionActive(Guid id)
    {
        return _sessions[id].StateManager.IsRunning();
    }

    public static GameManager Instance()
    {
        return _instance ??= new GameManager();
    }

    public bool SessionIsRunning(Guid id)
    {
        var session = _sessions[id];
        return session.StateManager.IsRunning();
    }

    public int GetSessionTime(Guid id)
    {
        return _sessions[id].StateManager.GetTime();
    }
}