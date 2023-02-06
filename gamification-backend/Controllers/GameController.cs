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
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        // GET: /api/CreateSession/
        [HttpGet]
        public string CreateSession(string username)
        {
            return _service.CreateSession(username);
        }

        // GET: /api/GetSessions/
        [HttpGet]
        public ActionResult<IEnumerable<GameSession>> GetSessions() // Kun for testing
        {
            return Ok(_service.GetSessions());
        }

        // POST: /api/SubmitTask/
        // FIXME: FIX ME
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask(string input)
        {
            return _service.SubmitTask(input);
        }

        // GET: /api/SelectTask
        [HttpGet]
        public GameTask SelectTask(int id)
        {
            return _service.SelectTask(id);
        }

        // GET: /api/GenerateTasks/
        [HttpGet]
        public List<GameTask> GenerateTasks()
        {
            return _service.GenerateTaskSet();
        }
    }
}