using GamificationBackend.Game;

namespace GamificationBackend.Models;

public class GameSession
{
    private readonly StateManager _stateManager;

    private GameTask? _currentTask;

    public GameSession(string name, int id, int startTime)
    {
        User = name;
        Id = id;
        _stateManager = new StateManager(startTime);
    }

    private int Id { get; } // Unique identifier for each session
    private string User { get; } // User class?

    public void StartNewTask(GameTask newTask)
    {
        _currentTask = newTask;
    }

    public TaskResult SubmitTask(string input)
    {
        if (_currentTask == null) throw new NullReferenceException("Error in GameSession.SubmitTask()");

        _currentTask.userCode = input;
        var res = GameLogic.Submit(_currentTask);

        if (res.Success)
        {
            //Update life and points in state
        }

        return res;
    }
}