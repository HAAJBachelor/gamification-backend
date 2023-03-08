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
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _service;

        public GameController(IGameService service, ILogger<GameController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public static string SessionId { get; } = "sessionId";

        // GET: /api/CreateSession/
        [HttpGet]
        public ActionResult<string> CreateSession()
        {
            if (Authorized()) return Ok("Already authorized");
            HttpContext.Session.SetInt32(SessionId, _service.CreateSession());
            _logger.LogInformation("Created new session with id " + GetSessionId());
            return Ok("A session was Created");
        }

        // POST: /api/SubmitTask/
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask([FromBody] string input)
        {
            if (!Authorized()) return Unauthorized();
            _logger.LogInformation("Submitting task for session " + GetSessionId());
            return Ok(_service.SubmitTask(GetSessionId(), input));
        }

        //POST: /api/SubmitTestCase/
        [HttpPost]
        public ActionResult<TestCaseResult> SubmitTestCase([FromBody] string input, int index)
        {
            if (!Authorized()) return Unauthorized();
            _logger.LogInformation("Submitting testcase for session " + GetSessionId());
            return _service.SubmitTestCase(GetSessionId(), input, index);
        }

        // GET: /api/SelectTask/
        [HttpGet]
        public ActionResult<GameTaskDTO> SelectTask(int taskId)
        {
            if (!Authorized()) return Unauthorized();
            _logger.LogInformation("Selecting task for session " + GetSessionId());
            return Ok(_service.SelectTask(GetSessionId(), taskId));
        }

        // GET: /api/GenerateTasks/
        [HttpGet]
        public ActionResult<List<GameTaskDTO>> GenerateTasks()
        {
            if (!Authorized()) return Unauthorized();
            _logger.LogInformation("Generating taskset for session " + GetSessionId());
            return Ok(_service.GenerateTaskSet(GetSessionId()));
        }

        // GET: /api/EndSession/
        [HttpGet]
        public ActionResult<string> EndSession()
        {
            _logger.LogInformation("Ending session with id: " + GetSessionId());
            return Ok("The session was ended");
        }

        // GET: /api/GetState/
        [HttpGet]
        public ActionResult<StateDTO> GetState()
        {
            if (!Authorized()) return Unauthorized();
            _logger.LogInformation("Fetching state for session " + GetSessionId());
            return Ok(_service.GetState(GetSessionId()));
        }

        // GET: /api/GetStartCode/
        [HttpGet]
        public ActionResult<string> GetStartCode(string language)
        {
            if (!Authorized()) return Unauthorized();
            if (Enum.TryParse(language, true, out StubGenerator.Language lang))
            {
                _logger.LogInformation("Getting startcode for session " + GetSessionId());
                return Ok(_service.GetStartCode(GetSessionId(), lang));
            }

            return NotFound($"Could not find language {language}");
        }

        // GET: /api/IsGameSessionActive
        [HttpGet]
        public ActionResult<bool> IsGameSessionActive()
        {
            if (!Authorized())
                return Unauthorized(false);
            return Ok(_service.IsGameSessionActive(GetSessionId()));
        }

        // GET: /api/SubmitUsername
        [HttpGet]
        public ActionResult<string> SubmitUsername(string username)
        {
            _service.SaveUsername(GetSessionId(), username);
            return Ok("Done");
        }

        private int GetSessionId()
        {
            return (int)HttpContext.Session.GetInt32(SessionId);
        }

        private bool Authorized()
        {
            return HttpContext.Session.GetInt32(SessionId) != null;
        }
    }
}