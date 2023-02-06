namespace gamification_backend.Models;

public class GameTask
{
    public GameTask(string description, int lives, int time)
    {
        Description = description;
        Lives = lives;
        Time = time;
        TestCases = new List<TestCase>();
    }

    public string Description { get; set; }
    public string UserCode { get; set; }
    public List<TestCase> TestCases { get; set; }

    public int Lives { get; set; }
    public int Time { get; set; } // Time as in time awarded for completing task
    public int Points { get; set; }

    public void AddSingleTestCase(TestCase testCase)
    {
        TestCases.Add(testCase);
    }

    public TestCase SingleTestCase()
    {
        return TestCases[0];
    }
}