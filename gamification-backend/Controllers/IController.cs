using gamification_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public interface IController
{
    public ActionResult<TaskResult> SubmitTask(string input);
}