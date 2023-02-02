using System.Text;
using GamificationBackend.Models;
using GamificationBackend.Service;

namespace GamificationBackend.Game;

public static class GameLogic
{
    public static TestResult RunTestCase(GameTask task)
    {
        var output = CodeCompiler.RunTask(task).Result;
        var result = ValidateTestCase(task.TestCases[0].output, output[0]);
        return result;
    }

    private static TestResult ValidateTestCase(string expected, string output)
    {
        var result = new TestResult();
        if (expected == output)
        {
            result.Success = true;
            result.Description = expected;
            return result;
        }

        //FIXME: We need to figure out how to know if we got an error
        if (expected.Contains("error"))
        {
            result.Error = true;
            result.Description = ConsolidateOutput(output);
            return result;
        }

        result.Description = $"You got {output}, expected {expected}";
        return result;
    }

    private static string ConsolidateOutput(string output)
    {
        const int maxLineLength = 20;
        var lines = output.Split("\n");
        if (lines.Length <= maxLineLength)
            return output;
        var builder = new StringBuilder();
        for (var i = 0; i < maxLineLength; i++) builder.Append(lines[i]).AppendLine();
        builder.Append("...");
        return builder.ToString();
    }
}