﻿using gamification_backend.DTO;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public class StateManager : IStateManager
{
    public enum RunningState
    {
        InTask,
        TaskSelect,
        GameEnded
    }

    private readonly Timer _timer;
    private int _lives;
    private int _points;

    private RunningState _runningState;

    public StateManager(int seconds, EventHandler<EventArgsFromTimer> handler)
    {
        _timer = new Timer(this, seconds, handler);
        _points = 0;
        _lives = 3; // Example amount
    }

    public void UpdateState(int lives, int time, int points)
    {
        UpdateLife(lives);
        AddTime(time);
        UpdatePoints(points);
    }

    //Returns state as a RunningState-object containing all data.
    public StateDTO GetState()
    {
        return new StateDTO(_points, _lives, _runningState);
    }

    public void EndGame()
    {
        _runningState = RunningState.GameEnded;
    }

    public void SetInTaskSelect()
    {
        _runningState = RunningState.TaskSelect;
    }

    public void StartSession()
    {
        SetInTaskSelect();
    }

    public void SetInTask()
    {
        _runningState = RunningState.InTask;
    }

    public bool InTask()
    {
        return _runningState == RunningState.InTask;
    }

    public bool InTaskSelect()
    {
        return _runningState == RunningState.TaskSelect;
    }

    public bool IsEnded()
    {
        return _runningState == RunningState.GameEnded;
    }

    public int GetTime()
    {
        return _timer.Seconds;
    }

    private void UpdateLife(int amount)
    {
        var newAmount = _lives + amount;
        if (newAmount < 0)
        {
            _lives = 0;
        }
        else _lives = newAmount;
    }

    private void UpdatePoints(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Cannot award less than 0 points.");
        _points += amount;
    }

    private void AddTime(int minutes)
    {
        if (minutes < 0) throw new ArgumentOutOfRangeException("Cannot award a negative amount of time");
        var seconds = minutes * 60;
        _timer.AddTime(seconds);
    }
}