using gamification_backend.Game;

namespace gamification_backend.Models;

public class GameManager
{
    private static GameManager _instance;

    //private static List<GameSession> sessions;
    private readonly Dictionary<int, GameSession> _sessions;
    private int _idCounter;

    private GameManager()
    {
        _sessions = new Dictionary<int, GameSession>();
        _idCounter = 0;
    }

    public static GameManager The()
    {
        if (_instance is null) _instance = new GameManager();
        return _instance;
    }

    public void CreateSession(string User = "TestString")
    {
        var session = new GameSession(User, _idCounter, 600);
        _sessions.Add(_idCounter, session);
        Console.WriteLine("Making new session, {0}", _sessions.Count);
        _idCounter++;
    }

    public Dictionary<int, GameSession> GetSessions()
    {
        return _sessions;
    }

    public TaskResult SubmitTask(string input)
    {
        var session = _sessions[0];
        return session.SubmitTask(input);
    }

    public void AddTask(GameTask gt)
    {
        Console.WriteLine(_sessions.Count);
        _sessions[0].StartNewTask(gt);
    }
}