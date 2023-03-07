﻿using gamification_backend.DTO;
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

    private readonly int _id;

    private readonly EventHandler<TimerDepletedEventArgs> _myEvent;
    private readonly StateManager _stateManager;
    private GameTask? _currentTask;
    private List<GameTask>? _taskSetToSelectFrom;
    private string _user; // User class?

    public GameSession(int id, int startTime, EventHandler<TimerDepletedEventArgs> eventHandler,
        string name = "placeholder")
    {
        _user = name;
        _id = id;
        _myEvent = eventHandler;
        TimerDepletedEvent += EndSession;
        _stateManager = new StateManager(startTime, TimerDepletedEvent);
        _stateManager.StartSession();
    }

    public GameTask StartNewTask(int id)
    {
        Console.WriteLine("Index: " + id);
        if (_taskSetToSelectFrom is not {Count: 3})
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
        _stateManager.UpdateState(rewards.Lives, rewards.Time, rewards.Points);

        return res;
    }

    public StateDTO GetState()
    {
        return _stateManager.GetState();
    }

    public event EventHandler TimerDepletedEvent;

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

    private void EndSession(object? sender, EventArgs args)
    {
        _stateManager.EndSession();
        var state = GetState();
        var record = new SessionRecord();
        record.Time = state._elapsed;
        record.Score = state._points;
        record.Id = _id;
        record.Username = 0;
        Console.WriteLine("Session expired");
        _myEvent.Invoke(this, new TimerDepletedEventArgs(record));
    }
}