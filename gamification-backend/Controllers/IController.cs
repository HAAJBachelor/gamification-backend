using gamification_backend.DTO;
using gamification_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public interface IController
{
    public ActionResult<string> CreateSession();
    public ActionResult<TaskResult> SubmitTask(string input);
    public ActionResult<TestCaseResult> SubmitTestCase(string input, int index);
    public ActionResult<GameTaskDTO> SelectTask(int taskId);
    public ActionResult<List<GameTask>> GenerateTasks();
    public ActionResult<string> EndSession();
    public ActionResult<StateDTO> GetState();
    public ActionResult<string> GetStartCode(string language);
}