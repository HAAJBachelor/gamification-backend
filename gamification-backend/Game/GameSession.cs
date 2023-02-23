using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

/// <summary>
///     An instance of a game for each user. Keeps track of everything related to that "playthrough".
/// </summary>
public class GameSession : IGameSession
{
    public delegate void EventHandler(object? sender, EventArgs args);

    private readonly EventHandler<TimerDepletedEventArgs> _del;
    private readonly int _id;
    private readonly StateManager _stateManager;
    private GameTask? _currentTask;
    private List<GameTask>? _taskSetToSelectFrom;
    private string _user; // User class?

    public GameSession(int id, int startTime, EventHandler<TimerDepletedEventArgs> eventHandler,
        string name = "placeholder")
    {
        _user = name;
        _id = id;
        _del = eventHandler;
        TimerDepletedEvent += EndSession;
        _stateManager = new StateManager(startTime, TimerDepletedEvent);
    }

    public GameTask StartNewTask(int id)
    {
        if (_taskSetToSelectFrom is { Count: 0 })
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

    public event EventHandler TimerDepletedEvent;

    private void EndSession(object? sender, EventArgs args)
    {
        var state = GetState();
        var record = new SessionRecord(_id, state._points, state._elapsed);
        Console.WriteLine("here");
        _del(this, new TimerDepletedEventArgs(record));
    }
}