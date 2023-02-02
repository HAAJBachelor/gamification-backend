using gamification_backend.Game;
using gamification_backend.Models;
using gamification_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService service)
        {
            _gameService = service;
        }

        // GET: /api/StartGame/
        [HttpGet]
        public string CreateSession(string username)
        {
            return _gameService.CreateSession(username);
        }

        // GET: /api/GetSessions/
        [HttpGet]
        public ActionResult<IEnumerable<GameSession>> GetSessions() // Kun for testing
        {
            return Ok(_gameService.GetSessions());
        }

        // POST: /api/SubmitTask
        // FIXME: FIX ME
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask(string input)
        {
            return _gameService.SubmitTask(input);
        }
    }
}