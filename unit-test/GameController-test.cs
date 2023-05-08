using System.Net;
using gamification_backend.Controllers;
using gamification_backend.Models;
using gamification_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace gamification_backend.Tests.Controllers
{
    public class GameControllerTests
    {
        private const string Active = "Active";
        private const string SessionId = "sessionId";
        private const string id = "12345678-1234-1234-1234-123456789012";
        private static readonly Mock<ILogger<GameController>> Mock = new();
        private static readonly Mock<IGameService> MockGameService = new();
        private readonly GameController _controller = new(MockGameService.Object, Mock.Object);
        private readonly Mock<HttpContext> _mockHttpContext = new();
        private readonly MockHttpSession _mockSession = new();


        //test for create session
        [Fact]
        public void Create_Session_OK()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.CreateSession(It.IsAny<Guid>()));
            //Act
            var result = _controller.CreateSession().Result as OkObjectResult;
            //Assert
            Assert.Equal("A session was Created", result?.Value);
            //Assert status code
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Create_Session_Already_Exists_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.CreateSession(It.IsAny<Guid>()));
            //Act
            var result = _controller.CreateSession().Result as OkObjectResult;
            //Assert
            Assert.Equal("A session was Created", result?.Value);
            //Assert status code
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        //test for submit task
        [Fact]
        public void Submit_Task_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            var taskResult = new TaskResult(new List<TestCaseResult>(), true, false);
            MockGameService.Setup(s => s.SubmitTask(It.IsAny<Guid>(), It.IsAny<string>())).Returns(taskResult);
            //Act
            var result = _controller.SubmitTask(It.IsAny<string>()).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
            Assert.Equal(taskResult, result?.Value);
        }

        [Fact]
        public void Submit_Task_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SubmitTask(It.IsAny<Guid>(), It.IsAny<string>()));
            //Act
            var result = _controller.SubmitTask(It.IsAny<string>()).Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test for submit test case
        [Fact]
        public void Submit_TestCase_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SubmitTestCase(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()));
            //Act
            var result = _controller.SubmitTestCase(It.IsAny<string>(), It.IsAny<int>()).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Submit_TestCase_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SubmitTestCase(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()));
            //Act
            var result =
                _controller.SubmitTestCase(It.IsAny<string>(), It.IsAny<int>()).Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test for submit test task test case
        [Fact]
        public void Submit_Test_Task_TestCase_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SubmitTestTaskTestCase(It.IsAny<string>(), It.IsAny<int>()));
            //Act
            var result =
                _controller.SubmitTestTaskTestCase(It.IsAny<string>(), It.IsAny<int>()).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        // test for select task
        [Fact]
        public void Select_Task_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SelectTask(It.IsAny<Guid>(), It.IsAny<int>()));
            //Act
            var result = _controller.SelectTask(It.IsAny<int>()).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Select_Task_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.SelectTask(It.IsAny<Guid>(), It.IsAny<int>()));
            //Act
            var result = _controller.SelectTask(It.IsAny<int>()).Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test for generate task
        [Fact]
        public void Generate_Tasks_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GenerateTaskSet(It.IsAny<Guid>()));
            //Act
            var result = _controller.GenerateTasks().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Generate_Tasks_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GenerateTaskSet(It.IsAny<Guid>()));
            //Act
            var result = _controller.GenerateTasks().Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test for get selected task
        [Fact]
        public void Get_Selected_Task_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GetSelectedTask(It.IsAny<Guid>())).Returns(new GameTask());
            //Act
            var result = _controller.GetSelectedTask().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Get_Selected_Task_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GetSelectedTask(It.IsAny<Guid>()));
            //Act
            var result = _controller.GetSelectedTask().Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        [Fact]
        public void Get_Selected_Task_Not_Found()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GetSelectedTask(It.IsAny<Guid>())).Returns(() => null);
            //Act
            var result = _controller.GetSelectedTask().Result as NotFoundObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.NotFound, result?.StatusCode);
        }

        //test for get leaderboard
        [Fact]
        public void Get_Leaderboard_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.GetLeaderboard());
            //Act
            var result = _controller.GetLeaderboard().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Get_State_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetState().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Get_State_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetState().Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test getstartcode
        [Fact]
        public void Get_Start_Code_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetStartCode("java", false).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Get_Start_Code_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetStartCode("java", false).Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        [Fact]
        public void Get_Start_Code_Test_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetStartCode("java", true).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Get_Start_Code_Test_Unathorized_OK()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.GetStartCode("java", true).Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Is_Game_Session_Active_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.IsGameSessionActive().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Is_Game_Session_Active_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.IsGameSessionActive().Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test skip task
        [Fact]
        public void Skip_Task_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            MockGameService.Setup(s => s.UseSkip(It.IsAny<Guid>()));
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.SkipTask().Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [Fact]
        public void Skip_Task_Unauthorized()
        {
            //Arrange
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            MockGameService.Setup(s => s.UseSkip(It.IsAny<Guid>()));
            //Act
            var result = _controller.SkipTask().Result as UnauthorizedObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result?.StatusCode);
        }

        //test submit username
        [Fact]
        public void Submit_Username_OK()
        {
            //Arrange
            _mockSession.SetString(Active, "Active");
            _mockSession.SetString(SessionId, id);
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            MockGameService.Setup(s => s.SaveUsername(It.IsAny<Guid>(), It.IsAny<string>()));
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
            //Act
            var result = _controller.SubmitUsername("test").Result as OkObjectResult;
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
            Assert.Equal("", _mockSession.GetString(Active));
        }
    }
}