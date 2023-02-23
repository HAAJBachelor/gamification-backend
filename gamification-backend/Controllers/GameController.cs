using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;
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
            if (Authorized()) return Ok();
            HttpContext.Session.SetInt32(_sessionId, _service.CreateSession());
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
            HttpContext.Session.SetInt32(_sessionId, -1);
            return Ok("The session was ended");
        }

        // GET: /api/GetState
        [HttpGet]
        public ActionResult<StateDTO> GetState()
        {
            return Ok(_service.GetState(GetSessionId()));
        }

        private int GetSessionId()
        {
            return (int)HttpContext.Session.GetInt32(_sessionId);
        }

        private bool Authorized()
        {
            return HttpContext.Session.GetInt32(_sessionId) != -1;
        }
    }
}