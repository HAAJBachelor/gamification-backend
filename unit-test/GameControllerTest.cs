using gamification_backend.Controllers;
using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;
using gamification_backend.Stub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace unit_test
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly GameController _gameController;
        private readonly MockHttpSession _mockSession;

        public GameControllerTests()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new MockHttpSession();
            _mockGameService = new Mock<IGameService>();
            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession);
            _gameController = new GameController(_mockGameService.Object)
            {
                ControllerContext = new ControllerContext {HttpContext = _mockHttpContext.Object}
            };
        }

        [Fact]
        public void CreateSession_Returns_OkResult_When_Not_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = null;
            // Act
            var result = _gameController.CreateSession();
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateSession_Returns_BadRequest_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.CreateSession();
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SubmitTask_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.SubmitTask("input");
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Authorize_Returns_True_When_SessionId_Is_Not_Null()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.Authorized();
            // Assert
            Assert.True(result);
        }

       
        [Fact]
        public void Authorize_Returns_False_When_SessionId_Is_Null()
        {
            // Arrange
            _mockSession["sessionId"] = null;
            // Act
            var result = _gameController.Authorized();
            // Assert
            Assert.False(result);
        }
        
          [Fact]
        public void SubmitTestCase_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.SubmitTestCase("input", 0);
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public void SelectTask_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.SelectTask(1);
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public void GenerateTasks_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GenerateTasks();
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public void GetSessionId_Returns_SessionId_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetSessionId();
            // Assert
            Assert.Equal(32, result);
        }
        
        [Fact]
        public void GetSessionId_Returns_NEGONE_When_Not_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = null;
            // Act
            var result = _gameController.GetSessionId();
            // Assert
            Assert.Equal(-1,result);
        }
        
        [Fact]
        public void GetState_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetState();
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public void GetStartCode_Returns_OkResult_When_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetStartCode("java");
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public void GetStartCode_Returns_UnAuthorized_When_Not_Authorized()
        {
            // Arrange
            _mockSession["sessionId"] = null;
            // Act
            var result = _gameController.GetStartCode("java");
            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
        
        [Fact]
        public void GetStartCode_Returns_BadRequest_When_Language_Is_Not_Supported()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetStartCode("c++");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        
        [Fact]
        public void GetStartCode_Returns_BadRequest_When_Language_Is_Null()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetStartCode(null);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        
        [Fact]
        public void GetStartCode_Returns_BadRequest_When_Language_Is_Empty()
        {
            // Arrange
            _mockSession["sessionId"] = 32;
            // Act
            var result = _gameController.GetStartCode("");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        
       
    }
}