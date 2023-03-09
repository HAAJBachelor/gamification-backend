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

    public void CreateSession(string id)
    {
        SaveSessionEventHandler += SaveSession;
        _manager.CreateSession(id, SaveSessionEventHandler);
    }

    public TaskResult SubmitTask(string sessionId, string input)
    {
        return _manager.SubmitTask(sessionId, input);
    }

    public TestCaseResult SubmitTestCase(string sessionId, string input, int index)
    {
        return _manager.SubmitTestCase(sessionId, input, index);
    }

    public List<GameTaskDTO> GenerateTaskSet(string sessionId)
    {
        var tasks = _repo.GenerateTaskSet();
        _manager.SaveTaskSet(sessionId, tasks.Result);
        return DTOMapper.GameTaskMapper(tasks.Result);
    }

    public GameTaskDTO SelectTask(string sessionId, int taskId)
    {
        return DTOMapper.GameTaskMapper(_manager.SelectTask(sessionId, taskId));
    }

    public StateDTO GetState(string sessionId)
    {
        return _manager.GetState(sessionId);
    }


    public string GetStartCode(string sessionId, StubGenerator.Language language)
    {
        return _manager.GetStartCode(sessionId, language);
    }

    public void SaveUsername(string sessionId, string username)
    {
        _repo.SaveUsername(sessionId, username);
    }

    public bool IsGameSessionActive(string sessionId)
    {
        return _manager.IsGameSessionActive(sessionId);
    }

    //public delegate void EventHandler(object? source, TimerDepletedEventArgs args);
    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        //_manager.RemoveSession(args.record.SessionId);
        _repo.SaveSession(args.record);
    }
}