using gamification_backend.DAL;
using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Service;

public class GameService : IGameService
{
    private readonly IGameManager _manager;
    private readonly IGameRepository _repo;

    public GameService(IGameRepository repo)
    {
        _manager = GameManager.Instance();
        _repo = repo;
    }

    public void CreateSession(Guid id)
    {
        SaveSessionEventHandler += SaveSession;
        _manager.CreateSession(id, SaveSessionEventHandler);
    }

    public TaskResult SubmitTask(Guid sessionId, string input)
    {
        return _manager.SubmitTask(sessionId, input);
    }

    public TestCaseResult SubmitTestCase(Guid sessionId, string input, int index)
    {
        return _manager.SubmitTestCase(sessionId, input, index);
    }

    public TestCaseResult SubmitTestTaskTestCase(string input, int index)
    {
        return _manager.SubmitTestTaskTestCase(input, index);
    }

    public List<GameTaskDTO> GenerateTaskSet(Guid sessionId)
    {
        var tasks = _repo.GenerateTaskSet();
        _manager.SaveTaskSet(sessionId, tasks.Result);
        return DTOMapper.GameTaskMapper(tasks.Result);
    }

    public GameTaskDTO SelectTask(Guid sessionId, int taskId)
    {
        return DTOMapper.GameTaskMapper(_manager.SelectTask(sessionId, taskId));
    }

    public StateDTO GetState(Guid sessionId)
    {
        return _manager.GetState(sessionId);
    }


    public string GetStartCode(Guid sessionId, StubGenerator.Language language)
    {
        return _manager.GetStartCode(sessionId, language);
    }

    public string GetTestTaskStartCode(StubGenerator.Language language)
    {
        return _manager.GetTestTaskStartCode(language);
    }

    public void SaveUsername(Guid sessionId, string username)
    {
        _repo.SaveUsername(sessionId, username);
    }

    public bool IsGameSessionActive(Guid sessionId)
    {
        return _manager.IsGameSessionActive(sessionId);
    }

    public GameTaskDTO SelectTaskForTesting(string taskId)
    {
        var gameTask = _repo.SelectTaskForTesting(taskId);
        _manager.TestTask = gameTask;
        var taskDTO = DTOMapper.GameTaskMapper(gameTask);
        gameTask.ValidatorCases.ForEach(t => taskDTO.TestCases.Add(t));
        return taskDTO;
    }

    public void EndSession(Guid sessionId)
    {
        _manager.RemoveSession(sessionId);
    }

    public GameTask GetSelectedTask(Guid getSessionId)
    {
        return _manager.GetSelectedTask(getSessionId);
    }

    //public delegate void EventHandler(object? source, TimerDepletedEventArgs args);
    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        _manager.RemoveSession(args.record.SessionId);
        _repo.SaveSession(args.record);
    }
}