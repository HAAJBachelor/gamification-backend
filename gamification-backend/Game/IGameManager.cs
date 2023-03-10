﻿using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Game;

public interface IGameManager
{
    public void CreateSession(Guid id, EventHandler<TimerDepletedEventArgs> eventHandler);
    public GameTask SelectTask(Guid sessionId, int taskId);
    public TaskResult SubmitTask(Guid sessionId, string input);
    public void SaveTaskSet(Guid sessionId, List<GameTask> tasks);
    public TestCaseResult SubmitTestCase(Guid sessionId, string input, int id);
    public StateDTO GetState(Guid sessionId);
    public void RemoveSession(Guid sessionId);
    public string GetStartCode(Guid sessionId, StubGenerator.Language language);
    bool IsGameSessionActive(Guid id);
}