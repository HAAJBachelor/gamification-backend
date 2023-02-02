namespace GamificationBackend.Models;

public class GameManager
{
    //private static List<GameSession> sessions;
    private readonly Dictionary<int, GameSession> _sessions;
    private int _idCounter;
    private static GameManager _instance;

    public static GameManager The()
    {
        if (_instance is null) _instance = new GameManager();
        return _instance;
    }
    private GameManager()
    {
        _sessions = new Dictionary<int, GameSession>();
        _idCounter = 0;
    }

    public void CreateSession(string User = "TestString")
    {
        GameSession session = new GameSession(User, _idCounter);
        _sessions.Add(_idCounter, session);
        _idCounter++;
    }

    public Dictionary<int, GameSession> GetSessions()
    {
        return _sessions;
    }

}