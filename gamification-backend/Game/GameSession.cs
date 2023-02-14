using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;
using gamification_backend.Stub;

namespace gamification_backend.Game;

public class GameSession
{
    public delegate void SessionDelegate();

    private readonly GameService.MyDel _del;
    private readonly int _id;
    private readonly StateManager _stateManager;
    private GameTask? _currentTask;
    private List<GameTask>? _taskSetToSelectFrom;
    private string _user; // User class?

    public GameSession(int id, int startTime, GameService.MyDel del, string name = "placeholder")
    {
        _user = name;
        _id = id;
        _del = del;
        SessionDelegate sessionDelegate = EndSession;
        _stateManager = new StateManager(startTime, sessionDelegate);
    }

    public GameTask StartNewTask(int id)
    {
        if (_taskSetToSelectFrom is not { Count: 2 })
        {
            throw new Exception(
                $"Error in GameSession.StartNewTask(), expected 3 tasks got {_taskSetToSelectFrom.Count}");
        }

        _currentTask = _taskSetToSelectFrom[id];
        _currentTask.StartCode = StubService.GenerateCode(_currentTask.StubCode, StubGenerator.Language.Java);
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

        if (!res.Success) return res;

        var rewards = _currentTask.Rewards;
        _stateManager.UpdateState(rewards.Lives, rewards.Time, rewards.Points);

        return res;
    }

    public StateDTO GetState()
    {
        return _stateManager.GetState();
    }

    private void EndSession()
    {
        var state = GetState();
        var record = new SessionRecord(_id, state._points, state._elapsed);
        _del(record);
    }
}