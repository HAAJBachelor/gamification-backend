using gamification_backend.DAL;
using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Stub;
using gamification_backend.Utility;

namespace gamification_backend.Service;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameManager _manager;
    private readonly ISessionRepository _sessionRepository;

    public GameService(IGameRepository repo, ISessionRepository sessionRepository)
    {
        _manager = GameManager.Instance();
        _gameRepository = repo;
        _sessionRepository = sessionRepository;
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
        var tasks = _gameRepository.GenerateTaskSet();
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
        _sessionRepository.SaveUsername(sessionId, username);
    }

    public bool IsGameSessionActive(Guid sessionId)
    {
        return _manager.IsGameSessionActive(sessionId);
    }

    public GameTaskDTO SelectTaskForTesting(string taskId)
    {
        var gameTask = _gameRepository.SelectTaskForTesting(taskId);
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

    public List<SessionRecordDTO> GetLeaderboard()
    {
        return _sessionRepository.GetLeaderboard().Result;
    }

    public void CancelSession(Guid sessionId)
    {
        _manager.CancelSession(sessionId);
    }

    //public delegate void EventHandler(object? source, TimerDepletedEventArgs args);
    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        _manager.RemoveSession(args.record.SessionId);
        _sessionRepository.SaveSession(args.record);
    }
}