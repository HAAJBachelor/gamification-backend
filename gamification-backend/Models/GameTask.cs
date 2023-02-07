namespace gamification_backend.Models;

public class GameTask
{
    private readonly TaskRewards _rewards;

    public GameTask(string description, int lives, int time)
    {
        Description = description;
        _rewards = new TaskRewards(lives, time);
        TestCases = new List<TestCase>();
    }

    public string Description { get; set; }
    public string UserCode { get; set; }
    public List<TestCase> TestCases { get; set; }

    public TaskRewards GetRewards()
    {
        return _rewards;
    }

    public void AddSingleTestCase(TestCase testCase)
    {
        TestCases.Add(testCase);
    }

    public TestCase SingleTestCase()
    {
        return TestCases[0];
    }

    public void SetPoints(int points)
    {
        _rewards.Points = points;
    }
}