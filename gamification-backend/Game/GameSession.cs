using GamificationBackend.Game;

namespace GamificationBackend.Models;

public class GameSession
{
    private readonly StateManager _stateManager;

    private GameTask? _currentTask;

    public GameSession(string name, int id, int startTime)
    {
        _user = name;
        _id = id;
        _stateManager = new StateManager(startTime);
    }

    private int _id { get; } // Unique identifier for each session
    private string _user { get; } // User class?

    public void StartNewTask(GameTask newTask)
    {
        _currentTask = newTask;
    }

    public TaskResult SubmitTask(string input)
    {
        if (_currentTask == null) throw new NullReferenceException("Error in GameSession.SubmitTask()");

        _currentTask.userCode = input;
        TaskResult res = GameLogic.RunTestCase(_currentTask);

        if (TaskResult.Success)
        {
            //Update life and points in state
        }
        else
        {
            return res;
        }
    }
}