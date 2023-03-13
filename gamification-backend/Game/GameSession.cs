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
    private readonly Guid _id;
    private readonly EventHandler<TimerDepletedEventArgs> _myEvent;
    private GameTask? _currentTask;
    private List<GameTask>? _taskSetToSelectFrom;

    public GameSession(Guid id, int startTime, EventHandler<TimerDepletedEventArgs> eventHandler)
    {
        _id = id;
        _myEvent = eventHandler;
        _timerEvent += EndSession;
        StateManager = new StateManager(startTime, _timerEvent);
        StateManager.StartSession();
    }

    public StateManager StateManager { get; }

    public GameTask StartNewTask(int id)
    {
        if (_taskSetToSelectFrom is not { Count: 3 })
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
        _currentTask.SessionId = _id;
        _currentTask.UserCode = input;
        var res = GameLogic.Submit(_currentTask);

        if (!res.Success) return res;

        var rewards = _currentTask.Rewards;
        StateManager.UpdateState(rewards.Lives, rewards.Time, rewards.Points);

        return res;
    }

    public StateDTO GetState()
    {
        return StateManager.GetState();
    }

    public event EventHandler<EventArgsFromTimer> _timerEvent;

    public TestCaseResult SubmitTestCase(string input, int index)
    {
        if (_currentTask == null) throw new NullReferenceException("Error in GameSession.SubmitTask()");
        _currentTask.UserCode = input;
        var res = GameLogic.RunTestCase(_currentTask, index);
        return res;
    }

    public GameTask? GetCurrentTask()
    {
        return _currentTask;
    }

    private void EndSession(object? sender, EventArgsFromTimer args)
    {
        StateManager.EndSession();
        var state = GetState();
        var record = new SessionRecord();
        var elapsed = args.StartTime - args.Seconds;
        record.Time = elapsed;
        record.Score = state._points;
        record.SessionId = _id;
        record.Username = "Anonym";
        Console.WriteLine("Session expired");
        _myEvent.Invoke(this, new TimerDepletedEventArgs(record));
    }
}