using gamification_backend.DAL;
using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;

namespace gamification_backend.Service;

public class GameService : IGameService
{
    public delegate void MyDel(SessionRecord s);

    private readonly GameManager _manager;
    private readonly IGameRepository _repo;

    public GameService(IGameRepository repo)
    {
        _manager = GameManager.Instance();
        _repo = repo;
    }

    public int CreateSession()
    {
        MyDel del = SaveSession;
        return _manager.CreateSession(del);
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

    private void SaveSession(SessionRecord session)
    {
        // Call repo to save the record
        Console.WriteLine("SaveSession in service");
        _manager.RemoveSession(session.Id);
    }
}