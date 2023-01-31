using GamificationBackend.Models;
using GamificationBackend.Service;
using Microsoft.AspNetCore.Mvc;

namespace GamificationBackend.Controllers
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
        
    }
}