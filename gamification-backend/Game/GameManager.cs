using gamification_backend.Models;

namespace gamification_backend.Game;

public class GameManager
{
    private static GameManager? _instance;

    //private static List<GameSession> sessions;
    private readonly Dictionary<int, GameSession> _sessions;
    private int _idCounter;

    private GameManager()
    {
        _sessions = new Dictionary<int, GameSession>();
    }

    public static GameManager Instance()
    {
        return _instance ??= new GameManager();
    }

    public int CreateSession()
    {
        var session = new GameSession(_idCounter, 600);
        _sessions.Add(_idCounter, session);
        Console.WriteLine("Creating new session with id {0}, total: {1}", _idCounter, _sessions.Count);
        return _idCounter++;
    }

    public GameTask SelectTask(int sessionId, int taskId)
    {
        return _sessions[sessionId].StartNewTask(taskId);
    }

    public TaskResult SubmitTask(int sessionId, string input)
    {
        var session = _sessions[sessionId];
        return session.SubmitTask(input);
    }

    public void SaveTaskSet(int sessionId, List<GameTask> tasks)
    {
        _sessions[sessionId].SaveGeneratedTaskSet(tasks);
    }
}