using gamification_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public interface IController
{
    public ActionResult<string> CreateSession();
    public ActionResult<TaskResult> SubmitTask(string input);
    public ActionResult<GameTask> SelectTask(int taskId);
    public ActionResult<List<GameTask>> GenerateTasks();
    public ActionResult<string> EndSession();
}