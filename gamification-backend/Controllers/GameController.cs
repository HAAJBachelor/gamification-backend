﻿using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;
using gamification_backend.Stub;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class GameController : Controller, IGameController
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _service;
        private string Active = "Active";

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
            if (Authorized()) _service.CancelSession(GetSessionId());
            HttpContext.Session.SetString(Active, "Active");
            HttpContext.Session.SetString(SessionId, GenKey());
            var res = _service.CreateSession(GetSessionId());
            if (!res)
            {
                _logger.LogInformation("Could not create session, already exists");
                return BadRequest("Something went wrong, could not create session");
            }

            _logger.LogInformation("Created new session with id " + GetSessionId());
            return Ok("A session was Created");
        }

        // POST: /api/SubmitTask/
        [HttpPost]
        public ActionResult<TaskResult> SubmitTask([FromBody] string input)
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Submitting task for session " + GetSessionId());
            return Ok(_service.SubmitTask(GetSessionId(), input));
        }

        //POST: /api/SubmitTestCase/
        [HttpPost]
        public ActionResult<TestCaseResult> SubmitTestCase([FromBody] string input, int index)
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Submitting testcase for session " + GetSessionId());
            return Ok(_service.SubmitTestCase(GetSessionId(), input, index));
        }

        //POST: /api/SubmitTestTaskTestCase/
        [HttpPost]
        public ActionResult<TestCaseResult> SubmitTestTaskTestCase([FromBody] string input, int index)
        {
            return Ok(_service.SubmitTestTaskTestCase(input, index));
        }

        // GET: /api/SelectTask/
        [HttpGet]
        public ActionResult<GameTaskDTO> SelectTask(int taskId)
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Selecting task for session " + GetSessionId());
            var task = _service.SelectTask(GetSessionId(), taskId);
            return Ok(task);
        }

        //GET: /api/GetSelectedTask
        [HttpGet]
        public ActionResult<GameTaskDTO> GetSelectedTask()
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Getting selected task for session " + GetSessionId());
            var task = _service.GetSelectedTask(GetSessionId());
            if (task == null)
            {
                _logger.LogInformation("The state says we're in a task, but no task is selected");
                return NotFound("No task is selected");
            }

            return Ok(task);
        }

        // GET: /api/SelectTaskForTesting/
        [HttpGet]
        public ActionResult<GameTaskDTO> SelectTaskForTesting(string taskId)
        {
            return Ok(_service.SelectTaskForTesting(taskId));
        }

        [HttpGet]
        public ActionResult<List<SessionRecordDTO>> GetLeaderboard()
        {
            return Ok(_service.GetLeaderboard());
        }

        // GET: /api/GenerateTasks/
        [HttpGet]
        public ActionResult<List<GameTaskDTO>> GenerateTasks()
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Generating taskset for session " + GetSessionId());
            var tasks = _service.GenerateTaskSet(GetSessionId());
            return Ok(tasks);
        }

        // GET: /api/GetState/
        [HttpGet]
        public ActionResult<StateDTO> GetState()
        {
            if (!Authorized()) return Unauthorized("No session found");
            _logger.LogInformation("Fetching state for session " + GetSessionId());
            return Ok(_service.GetState(GetSessionId()));
        }

        // GET: /api/GetStartCode/
        [HttpGet]
        public ActionResult<string> GetStartCode(string language, bool test)
        {
            if (Enum.TryParse(language, true, out StubGenerator.Language lang))
            {
                if (test) return Ok(_service.GetTestTaskStartCode(lang));

                if (!Authorized()) return Unauthorized("No session found");

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

        // GET: /api/UseSkip
        [HttpGet]
        public ActionResult<bool> SkipTask()
        {
            if (!Authorized()) return Unauthorized("No session found");
            return Ok(_service.UseSkip(GetSessionId()));
        }

        // GET: /api/SubmitUsername
        [HttpGet]
        public ActionResult<string> SubmitUsername(string username)
        {
            if (!Authorized()) return Unauthorized();
            _service.SaveUsername(GetSessionId(), username);
            EndSession();
            return Ok("Done");
        }

        public Guid GetSessionId()
        {
            var id = HttpContext.Session.GetString(SessionId);
            return Guid.Parse(id!);
        }

        private bool Authorized()
        {
            try
            {
                return !string.IsNullOrEmpty(HttpContext.Session.Get(Active)!.ToString());
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void EndSession()
        {
            _logger.LogInformation("Ending session with id: " + GetSessionId());
            HttpContext.Session.SetString(Active, "");
        }


        private string GenKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}