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

        public GameController(IGameService service)
        {
            _service = service;
        }

        // GET: /api/CreateSession/
        [HttpGet]
        public ActionResult<string> CreateSession()
        {
            if (Authorized()) return BadRequest("Already authorized");
            HttpContext.Session.SetInt32(_sessionId, _service.CreateSession());
            return Ok("A session was Created");
        }

        // POST: /api/SubmitTask/
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask([FromBody] string input)
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.SubmitTask(GetSessionId(), input));
        }

        //POST: /api/SubmitTestCase/
        [HttpPost]
        public ActionResult<TestCaseResult> SubmitTestCase([FromBody] string input, int index)
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.SubmitTestCase(GetSessionId(), input, index));
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
            HttpContext.Session.Remove(_sessionId);
            return Ok("The session was ended");
        }

        // GET: /api/GetState/
        [HttpGet]
        public ActionResult<StateDTO> GetState()
        {
            if (!Authorized()) return Unauthorized();
            return Ok(_service.GetState(GetSessionId()));
        }

        // GET: /api/GetStartCode/
        [HttpGet]
        public ActionResult<string> GetStartCode(string language)
        {
            if (!Authorized()) return Unauthorized();
            if (language == "")
                return BadRequest("No language was specified");
            if (Enum.TryParse(language, true, out StubGenerator.Language lang))
                return Ok(_service.GetStartCode(GetSessionId(), lang));
            return BadRequest($"Could not find language {language}");
        }

        public int GetSessionId()
        {
            if (HttpContext.Session.GetString(_sessionId) == null)
                return -1;
            return Convert.ToInt32(HttpContext.Session.GetString(_sessionId));
        }

        public bool Authorized()
        {
            return HttpContext.Session.GetString(_sessionId) != null;
        }

        public string GetSes()
        {
            return _sessionId;
        }
    }
}