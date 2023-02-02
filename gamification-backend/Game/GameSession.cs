using System.Diagnostics;

namespace GamificationBackend.Models;

public class GameSession
{
    //verdier er public kun for testing...
    public int Id { get; set; } // Unique identifier for each session
    public string User { get; set; } // Username or some other identifier? May be replaced with a user-object or user-id.

    private StateManager StateManager;
    public GameSession(string name, int id)
    {
        User = name;
        Id = id;
        StateManager = new StateManager();

    }
}