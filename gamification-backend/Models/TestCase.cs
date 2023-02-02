namespace GamificationBackend.Models;

public struct TestCase
{
    public TestCase(string input, string output)
    {
        this.input = input;
        this.output = output;
    }

    public string input { get; }
    public string output { get; }
}