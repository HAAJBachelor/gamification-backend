using gamification_backend.DAL;
using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Sanity;
using gamification_backend.Service;
using gamification_backend.Utility;
using Moq;
using Task = gamification_backend.Sanity.Task;

namespace unit_test;

public class GameService_test
{
    private static readonly Mock<IGameRepository> _gameRepository = new();
    private static readonly Mock<IGameManager> _manager = new();
    private static readonly Mock<ISessionRepository> _sessionRepository = new();

    private readonly GameService _gameService =
        new GameService(_gameRepository.Object, _sessionRepository.Object, _manager.Object);

    private List<Task> GenerateTasks(int amount)
    {
        var tasks = new List<Task>();
        for (var i = 0; i < amount; i++)
        {
            var task = new Task
            {
                _id = "id_" + i,
                Title = "title_" + i,
                Description = "description_" + i,
                InputDescription = "inputDescription_" + i,
                OutputDescription = "outputDescription_" + i,
                Constraints = "constraints_" + i,
                TestCases = new Case[]
                {
                    new Case
                    {
                        TestCaseInput = "1",
                        TestCaseOutput = "1",
                        ValidatorInput = "2",
                        ValidatorOutput = "2"
                    }
                },
                Stub = "read n:int\nwrite n",
                Rewards = new Reward[]
                {
                    new Reward
                    {
                        Type = "Time",
                        Amount = i
                    }
                },
                Difficulty = "Easy",
                Category = new Category[]
                {
                    new Category
                    {
                        Name = "Arrays"
                    }
                },
                Score = 10
            };
            tasks.Add(task);
        }

        return tasks;
    }

    //test generateTaskSet
    [Fact]
    public void Generate_TaskSet_OK()
    {
        //arrange
        var tasks = GenerateTasks(10);
        var guid = Guid.NewGuid();
        _gameRepository.Setup(x => x.GenerateTaskSet()).ReturnsAsync(tasks);
        var finishedTasks = new List<string>();
        _manager.Setup(x => x.FinishedTasks(guid)).Returns(finishedTasks);
        _manager.Setup(x => x.SaveTaskSet(guid, It.IsAny<List<GameTask>>()));
        //act
        var result = _gameService.GenerateTaskSet(guid);
        //assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Generate_TaskSet_All_Finished_OK()
    {
        //arrange
        var tasks = GenerateTasks(10);
        var guid = Guid.NewGuid();
        _gameRepository.Setup(x => x.GenerateTaskSet()).ReturnsAsync(tasks);
        var finishedTasks = tasks.Select(task => task._id).ToList();
        _manager.Setup(x => x.FinishedTasks(guid)).Returns(finishedTasks);
        _manager.Setup(x => x.SaveTaskSet(guid, It.IsAny<List<GameTask>>()));
        //act
        var result = _gameService.GenerateTaskSet(guid);
        //assert
        Assert.Equal(3, result.Count);
    }
}