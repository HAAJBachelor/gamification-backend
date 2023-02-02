namespace GamificationBackend.Models;

public class GameTask
{
    public string Description { get; set; }
    public| string userCode { get; set; }
    public List<TestCase> TestCases { get; set; }

    public void addSingleTestCase(TestCase testCase)
    {
        TestCases.Add(testCase);
    }

    public TestCase SingleTestCase()
    {
        return TestCases[0];
    }
}