﻿using gamification_backend.Models;

namespace gamification_backend.DTO;

public class GameTaskDTO
{
    private readonly TaskRewards _rewards;

    public int TaskId { get; set; }
    public string Description { get; set; }
    public string StartCode { get; set; }
    public List<TestCase> TestCases { get; set; }

    public TaskRewards Rewards { get; set; }
}