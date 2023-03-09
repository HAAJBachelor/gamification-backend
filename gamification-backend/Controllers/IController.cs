﻿using gamification_backend.DTO;
using gamification_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public interface IController
{
    public ActionResult<string> CreateSession();
    public ActionResult<TaskResult> SubmitTask(string input);
    public ActionResult<TestCaseResult> SubmitTestCase(string input, int index);
    public ActionResult<GameTaskDTO> SelectTask(int taskId);
    public ActionResult<List<GameTaskDTO>> GenerateTasks();
    public ActionResult<StateDTO> GetState();
    public ActionResult<string> GetStartCode(string language);

    public ActionResult<bool> IsGameSessionActive();
}