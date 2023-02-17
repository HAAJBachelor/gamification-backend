using gamification_backend.DAL;
using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Utility;

namespace gamification_backend.Service;

public class GameService : IGameService
{
    public delegate void EventHandler(object? source, TimerDepletedEventArgs args);

    private readonly GameManager _manager;
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

    public event EventHandler<TimerDepletedEventArgs> SaveSessionEventHandler;

    private void SaveSession(object? source, TimerDepletedEventArgs args)
    {
        Console.WriteLine("SaveSession in service");
        _manager.RemoveSession(args.record.Id);
        _repo.SaveSession(args.record);
    }
}