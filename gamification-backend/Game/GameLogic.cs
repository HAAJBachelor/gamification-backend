using System.Text;
using gamification_backend.Models;
using gamification_backend.Service;

namespace gamification_backend.Game;

public static class GameLogic
{
    public static TestCaseResult RunTestCase(GameTask task, int index)
    {
        var output = CodeCompiler.Instance().RunTask(task).Result;
        if (output.Error) Console.WriteLine(output.Error_message);
        var result = ValidateTestCase(task.SingleTestCase(index).Output, output.Results[index]);
        return result;
    }

    public static TaskResult Submit(GameTask task)
    {
        var outputs = CodeCompiler.Instance().RunTask(task).Result;
        var testCaseResults = outputs.Results
            .Select((t, i) =>
                ValidateTestCase(task.TestCases[i].Output, t))
            .ToList();
        var success = testCaseResults.All(testCaseResult => testCaseResult.Success);
        return new TaskResult(testCaseResults, success);
    }

    private static TestCaseResult ValidateTestCase(string expected, string output)
    {
        var result = new TestCaseResult();
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