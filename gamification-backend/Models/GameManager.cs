namespace GamificationBackend.Models;

public static class GameManager
{
    private static List<GameSession> sessions;
    private static int idCounter;

    static GameManager()
    {
        sessions = new List<GameSession>();
        idCounter = 0;
    }

    public static void CreateSession(string User = "TestString")
    {
        GameSession session = new GameSession(User, idCounter);
        sessions.Add(session);
        idCounter++;
    }

    public static List<GameSession> GetSessions()
    {
        return sessions;
    }

}