using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public class GameManager : IGameManager
{
    private static GameManager? _instance;
    private readonly Dictionary<int, GameSession> _sessions;
    private int _idCounter;

    private GameManager()
    {
        _sessions = new Dictionary<int, GameSession>();
    }

    public int CreateSession(EventHandler<TimerDepletedEventArgs> eventHandler)
    {
        var session = new GameSession(_idCounter, 600, eventHandler);
        _sessions.Add(_idCounter, session);
        Console.WriteLine("Creating new session with id {0}, total: {1}", _idCounter, _sessions.Count);
        return _idCounter++;
    }

    public GameTask SelectTask(int sessionId, int taskId)
    {
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].StartNewTask(taskId);
        throw new ArgumentException("Invalid session Id");
    }

    public TaskResult SubmitTask(int sessionId, string input)
    {
        if (!_sessions.ContainsKey(sessionId)) throw new ArgumentException("Invalid session Id");
        var session = _sessions[sessionId];
        return session.SubmitTask(input);
    }

    public void SaveTaskSet(int sessionId, List<GameTask> tasks)
    {
        if (_sessions.ContainsKey(sessionId))
            _sessions[sessionId].SaveGeneratedTaskSet(tasks);
        else throw new ArgumentException("Invalid session Id " + sessionId);
    }

    public StateDTO GetState(int sessionId)
    {
        Console.WriteLine("Fetching session state for {0}", sessionId);
        if (_sessions.ContainsKey(sessionId))
            return _sessions[sessionId].GetState();
        throw new ArgumentException("Invalid session Id");
    }

    public void RemoveSession(int sessionId)
    {
        _sessions.Remove(sessionId);
    }

    public static GameManager Instance()
    {
        return _instance ??= new GameManager();
    }
}