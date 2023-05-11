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

    public GameService(IGameRepository repo, ISessionRepository sessionRepository, IGameManager manager)
    {
        _manager = manager;
        _gameRepository = repo;
        _sessionRepository = sessionRepository;
    }

    public bool CreateSession(Guid id)
    {
        SaveSessionEventHandler += SaveSession;
        return _manager.CreateSession(id, SaveSessionEventHandler);
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
        if (_manager.HasGeneratedTaskSet(sessionId))
            return DTOMapper.GameTaskMapper(_manager.GetGeneratedTaskSet(sessionId)!);
        var finishedTasks = _manager.FinishedTasks(sessionId);
        var sanityTask = _gameRepository.GenerateTaskSet().Result;
        var filteredTasks = sanityTask.Where(t => !finishedTasks.Contains(t._id)).ToList();
        var tasks = new List<GameTask>();
        var rnd = new Random();
        var randomIndexes = Enumerable.Range(0, filteredTasks.Count).OrderBy(x => rnd.Next()).ToArray();
        tasks.AddRange(randomIndexes
            .Select(index => TaskMapper.FromSanityTaskToGameTask(filteredTasks[index]))
            .Where(t => !finishedTasks.Contains(t.Id))
            .Take(3));
        if (tasks.Count < 3)
        {
            randomIndexes = Enumerable.Range(0, sanityTask.Count).OrderBy(x => rnd.Next()).ToArray();
            var i = 0;
            while (tasks.Count < 3)
            {
                var t = TaskMapper.FromSanityTaskToGameTask(sanityTask[randomIndexes[i++]]);
                if (tasks.Contains(t))
                    continue;
                tasks.Add(t);
            }
        }

        _manager.SaveTaskSet(sessionId, tasks);
        return DTOMapper.GameTaskMapper(tasks);
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

    public bool UseSkip(Guid sessionId)
    {
        return _manager.UseSkip(sessionId);
    }

    //public delegate void EventHandler(object? source, TimerDepletedEventArgs args);
    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        _manager.RemoveSession(args.record.SessionId);
        _sessionRepository.SaveSession(args.record);
    }
}