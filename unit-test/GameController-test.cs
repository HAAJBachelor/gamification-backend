using gamification_backend.Controllers;
using gamification_backend.DAL;
using gamification_backend.Service;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sanity.Linq.Extensions;

namespace unit_test;
public class GameController_test
{
    private readonly string _sessionId = "sessionId";
    
    private readonly ITestOutputHelper _testOutputHelper;
    private static readonly Mock<IGameRepository> Repo = new Mock<IGameRepository>();
    private static readonly Mock<IGameService> Service = new Mock<IGameService>();
    private readonly GameController _controller = new(Service.Object, new Mock<ILogger<GameController>>().Object);

    // private static readonly Mock<HttpContext> MockHttpContext = new Mock<HttpContext>();

    public GameController_test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateSession_Ok()
    {
        var result = Service.Object.CreateSession();
        _testOutputHelper.WriteLine("Print: " + result);
        Assert.Equal(0, result);
    }
}