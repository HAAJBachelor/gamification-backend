using gamification_backend.Models;

namespace gamification_backend.Game;

public class GameSession
{
    private readonly StateManager _stateManager;

    private GameTask? _currentTask;

    private int _id; // Unique identifier for each session
    private string _user; // User class?

    public GameSession(string name, int id, int startTime)
    {
        _user = name;
        _id = id;
        _stateManager = new StateManager(startTime);
    }

    public void StartNewTask(GameTask newTask)
    {
        _currentTask = newTask;
    }

    public TaskResult SubmitTask(string input)
    {
        if (_currentTask == null) throw new NullReferenceException("Error in GameSession.SubmitTask()");

        _currentTask.UserCode = input;
        var res = GameLogic.Submit(_currentTask);

        if (res.Success)
        {
            //Update life and points in state
        }

        return res;
    }
}