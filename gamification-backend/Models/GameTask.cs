namespace gamification_backend.Models;

public class GameTask
{
    public GameTask()
    {
        TestCases = new List<TestCase>();
    }

    public string Description { get; set; }
    public string UserCode { get; set; }
    public List<TestCase> TestCases { get; set; }

    public void AddSingleTestCase(TestCase testCase)
    {
        TestCases.Add(testCase);
    }

    public TestCase SingleTestCase()
    {
        return TestCases[0];
    }
}