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
        public ActionResult<string> CreateSession()
        {
            if (Authorized()) return Ok("Session already exists");
            HttpContext.Session.SetInt32(_sessionId, _service.CreateSession());
            HttpContext.Session.SetString(_valid, "valid");
            Console.WriteLine(HttpContext.Session.GetInt32(_sessionId));
            return Ok();
        }

        // POST: /api/SubmitTask/
        // FIXME: FIX ME
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask(string input)
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.SubmitTask(GetSessionId(), input));
        }

        // GET: /api/SelectTask/
        [HttpGet]
        public ActionResult<GameTask> SelectTask(int taskId)
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.SelectTask(GetSessionId(), taskId));
        }

        // GET: /api/GenerateTasks/
        [HttpGet]
        public ActionResult<List<GameTask>> GenerateTasks()
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.GenerateTaskSet(GetSessionId()));
        }

        // GET: /api/EndSession/
        [HttpGet]
        public ActionResult<string> EndSession()
        {
            HttpContext.Session.SetString(_valid, "");
            return Ok();
        }

        private int GetSessionId()
        {
            return (int)HttpContext.Session.GetInt32(_sessionId);
        }

        private bool Authorized()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_valid))) return false;
            return true;
        }
    }
}