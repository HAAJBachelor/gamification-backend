namespace GamificationBackend.Models;

public struct TestCase
{
    public TestCase(string input, string output)
    {
        Input = input;
        Output = output;
    }

    public string Input { get; }
    public string Output { get; }
}