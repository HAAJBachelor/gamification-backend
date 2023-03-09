using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public class GameManager : IGameManager
{
    private static GameManager? _instance;
    private readonly Dictionary<string, GameSession> _sessions;

    private GameManager()
    {
        _sessions = new Dictionary<string, GameSession>();
    }

    public void CreateSession(string id, EventHandler<TimerDepletedEventArgs> eventHandler)
    {
        var session = new GameSession(id, 300, eventHandler);
        _sessions.Add(id, session);
        Console.WriteLine("Creating new session with id {0}, total: {1}", id, _sessions.Count);
    }

    public GameTask SelectTask(string sessionId, int taskId)
    {
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].StartNewTask(taskId);
        throw new ArgumentException("Invalid session Id. GameManager.SelectTask");
    }

    public TaskResult SubmitTask(string sessionId, string input)
    {
        if (!_sessions.ContainsKey(sessionId)) throw new ArgumentException("Invalid session Id");
        var session = _sessions[sessionId];
        return session.SubmitTask(input);
    }

    public TestCaseResult SubmitTestCase(string sessionId, string input, int id)
    {
        return _sessions[sessionId].SubmitTestCase(input, id);
    }

    public void SaveTaskSet(string sessionId, List<GameTask> tasks)
    {
        if (_sessions.ContainsKey(sessionId))
            _sessions[sessionId].SaveGeneratedTaskSet(tasks);
        else throw new ArgumentException("Invalid session Id " + sessionId);
    }

    public StateDTO GetState(string sessionId)
    {
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].GetState();
        throw new ArgumentException("Invalid session Id");
    }

    public void RemoveSession(string sessionId)
    {
        _sessions.Remove(sessionId);
    }

    public string GetStartCode(string sessionId, StubGenerator.Language language)
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

    public bool IsGameSessionActive(string id)
    {
        return _sessions[id].StateManager.IsRunning();
    }

    public static GameManager Instance()
    {
        return _instance ??= new GameManager();
    }

    public bool SessionIsRunning(string id)
    {
        var session = _sessions[id];
        return session.StateManager.IsRunning();
    }

    public int GetSessionTime(string id)
    {
        return _sessions[id].StateManager.GetTime();
    }
}