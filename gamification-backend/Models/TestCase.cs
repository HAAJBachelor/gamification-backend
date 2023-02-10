namespace gamification_backend.Models;

public struct TestCase
{
    public TestCase(string input, string output)
    {
        Input = input;
        Output = output;
    }

    public string Input { get; set; }
    public string Output { get; set; }
}