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

    public int CreateSession()
    {
        SaveSessionEventHandler += SaveSession;
        return _manager.CreateSession(SaveSessionEventHandler);
    }

    public TaskResult SubmitTask(int sessionId, string input)
    {
        return _manager.SubmitTask(sessionId, input);
    }

    public TestCaseResult SubmitTestCase(int sessionId, string input, int index)
    {
        return _manager.SubmitTestCase(sessionId, input, index);
    }

    public TestCaseResult SubmitTestTaskTestCase(string input, int index)
    {
        return _manager.SubmitTestTaskTestCase(input, index);
    }

    public List<GameTaskDTO> GenerateTaskSet(int sessionId)
    {
        var tasks = _repo.GenerateTaskSet();
        _manager.SaveTaskSet(sessionId, tasks.Result);
        return DTOMapper.GameTaskMapper(tasks.Result);
    }

    public GameTaskDTO SelectTask(int sessionId, int taskId)
    {
        return DTOMapper.GameTaskMapper(_manager.SelectTask(sessionId, taskId));
    }

    public StateDTO GetState(int sessionId)
    {
        return _manager.GetState(sessionId);
    }


    public string GetStartCode(int sessionId, StubGenerator.Language language)
    {
        return _manager.GetStartCode(sessionId, language);
    }

    public string GetTestTaskStartCode(StubGenerator.Language language)
    {
        return _manager.GetTestTaskStartCode(language);
    }

    public void SaveUsername(int sessionId, string username)
    {
        _repo.SaveUsername(sessionId, username);
    }

    public bool IsGameSessionActive(int sessionId)
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

    //public delegate void EventHandler(object? source, TimerDepletedEventArgs args);
    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        _manager.RemoveSession(args.record.Id);
        var x = _repo.SaveSession(args.record);
    }
}