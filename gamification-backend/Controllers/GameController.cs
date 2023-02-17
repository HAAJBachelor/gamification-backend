using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;
using gamification_backend.Stub;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class GameController : Controller, IController
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
            if (Authorized()) return BadRequest("Session already exists");
            HttpContext.Session.SetInt32(_sessionId, _service.CreateSession());
            HttpContext.Session.SetString(_valid, "valid");
            return Ok("A session was Created");
        }

        // POST: /api/SubmitTask/
        // FIXME: FIX ME
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask([FromBody] string input)
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.SubmitTask(GetSessionId(), input));
        }

        // GET: /api/SelectTask/
        [HttpGet]
        public ActionResult<GameTaskDTO> SelectTask(int taskId)
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
            return Ok("The session was ended");
        }

        // GET: /api/GetState
        [HttpGet]
        public ActionResult<StateDTO> GetState()
        {
            return Ok(_service.GetState(GetSessionId()));
        }

        [HttpGet]
        public ActionResult<string> GetStartCode(string language)
        {
            if (Enum.TryParse(language, true, out StubGenerator.Language lang))
                return Ok(_service.GetStartCode(GetSessionId(), lang));
            return NotFound($"Could not find language {language}");
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