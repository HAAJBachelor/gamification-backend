using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public class GameManager : IGameManager
{
    private static GameManager? _instance;
    private int _idCounter;
    private GameTask? _testTask;
    private readonly Dictionary<Guid, GameSession> _sessions;

    private GameManager()
    {
        _sessions = new Dictionary<Guid, GameSession>();
    }

    public void CreateSession(Guid id, EventHandler<TimerDepletedEventArgs> eventHandler)
    {
        var session = new GameSession(id, 600, eventHandler);
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
    
    public TestCaseResult SubmitTestTaskTestCase(string input, int index)
    {
        if (TestTask == null) throw new ArgumentException("Test task is not set");
        TestTask.UserCode = input;
        return GameLogic.RunTestCase(TestTask, index);
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

    public string GetTestTaskStartCode(StubGenerator.Language language)
    {
        if (_testTask == null)
            throw new ArgumentException("Test task is not set");
        var code = StubService.GenerateCode(_testTask.StubCode, language);
        _testTask.StartCode = code;
        _testTask.Language = language.ToString().ToLower();
        return code;
    }
    
    public bool IsGameSessionActive(Guid id)
    {
        return _sessions[id].StateManager.IsRunning();
    }

    public GameTask? TestTask
    {
        get => _testTask;
        set
        {
            if (_testTask is null)
            {
                _testTask = value;
                return;
            }

            var lang = _testTask.Language;
            _testTask = value;
            if (lang != "")
                _testTask.Language = lang;
        }
    }

    public GameTask? GetSelectedTask(Guid getSessionId)
    {
        return _sessions[getSessionId].GetCurrentTask();
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
