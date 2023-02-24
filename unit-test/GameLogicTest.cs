using gamification_backend.DTO;
using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Service;
using Moq;

namespace unit_test;

public class GameLogicTest
{
    
    private readonly Mock<ICodeCompiler> _mockCompiler = new();
    
    
    [Fact]
    public void RunTestCase()
    {
        _mockCompiler.Setup(c => c.RunTask(It.IsAny<GameTask>(),1))
            .ReturnsAsync(new CompilerResultsDTO
            {
                Error = false,
                Results = new List<TestCaseResult>
                {
                    new()
                    {
                        Success = true,
                        Error = false,
                        Description = "Hello World"
                    }
                }
            });
        var results = GameLogic.RunTestCase(new GameTask(), 0);
        Assert.True(results.Success);
    }
    
}