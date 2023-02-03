using gamification_backend.Models;

namespace gamification_backend.Game;

public class GameSession
{
    private readonly StateManager _stateManager;

    private GameTask? _currentTask;

    private int _id; // Unique identifier for each session
    private string _user; // User class?

    private List<GameTask> _taskSetToSelectFrom;

    public GameSession(string name, int id, int startTime)
    {
        _user = name;
        _id = id;
        _stateManager = new StateManager(startTime);
    }

    public GameTask StartNewTask(int id)
    {
        if (_taskSetToSelectFrom is not { Count: 3 })
        {
            throw new Exception("Error in GameSession.StartNewTask()");
        }
        _currentTask = _taskSetToSelectFrom[id];
        _taskSetToSelectFrom.Clear();

        return _currentTask;
    }

    public void SaveGeneratedTaskSet(List<GameTask> tasks)
    {
        _taskSetToSelectFrom = tasks;
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