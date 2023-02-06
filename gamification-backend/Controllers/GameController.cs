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
        private readonly string _sessionId = "sessionId";
        private readonly string _valid = "valid";


        public GameController(IGameService service)
        {
            _service = service;
        }

        // GET: /api/CreateSession/
        [HttpGet]
        public ActionResult<string> CreateSession(string username)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(_valid))) return Ok("Session already exists");
            HttpContext.Session.SetInt32(_sessionId, _service.CreateSession());
            HttpContext.Session.SetString(_valid, "valid");
            Console.WriteLine(HttpContext.Session.GetInt32(_sessionId));
            return Ok();
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
            return Ok(_service.SubmitTask(input));
        }

        // GET: /api/SelectTask/
        [HttpGet]
        public ActionResult<GameTask> SelectTask(int id)
        {
            return Ok(_service.SelectTask(id));
        }

        // GET: /api/GenerateTasks/
        [HttpGet]
        public ActionResult<List<GameTask>> GenerateTasks()
        {
            var valid = HttpContext.Session.GetString(_valid);
            if (string.IsNullOrEmpty(valid))
            {
                Console.WriteLine("Session is Invalid");
                return Unauthorized();
            }

            Console.WriteLine(HttpContext.Session.GetInt32(_sessionId));
            return Ok(_service.GenerateTaskSet());
        }

        // GET: /api/EndSession/
        [HttpGet]
        public ActionResult<string> EndSession()
        {
            HttpContext.Session.SetString(_valid, "");
            return Ok();
        }
    }
}