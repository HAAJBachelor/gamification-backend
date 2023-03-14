using gamification_backend.Stub;

namespace gamification_backend.Models;

/// <summary>
///     A task given to a user. Has a list of testcases.
/// </summary>
public class GameTask
{
    private readonly TaskRewards _rewards;

    public GameTask()
    {
    }

    public GameTask(string description, int lives, int time)
    {
        Description = description;
        _rewards = new TaskRewards
        {
            Lives = lives,
            Time = time
        };
        TestCases = new List<TestCase>();
    }

    public string? Id { get; set; }

    public int TaskId { get; set; }

    public Guid SessionId { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string InputDescription { get; set; }
    public string OutputDescription { get; set; }
    public string Constraints { get; set; }
    public string UserCode { get; set; }

    public string StubCode { get; set; }

    public string StartCode { get; set; }
    public List<TestCase> TestCases { get; set; }
    public List<TestCase> ValidatorCases { get; set; }
    public string Difficulty { get; set; }

    public string[] Category { get; set; }
    public TaskRewards Rewards { get; set; }

    public StubGenerator.Language LanguageAsEnum()
    {
        if (Enum.TryParse(Language, true, out StubGenerator.Language lang))
        {
            return lang;
        }

        return StubGenerator.Language.None;
    }

    public void AddSingleTestCase(TestCase testCase)
    {
        TestCases.Add(testCase);
    }

    public TestCase SingleTestCase(int index)
    {
        return TestCases[index];
    }

    public void SetPoints(int points)
    {
        _rewards.Points = points;
    }
}