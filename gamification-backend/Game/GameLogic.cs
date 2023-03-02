using System.Text;
using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;

namespace gamification_backend.Game;

public static class GameLogic
{
    public static TestCaseResult RunTestCase(GameTask task, int index)
    {
        var output = CodeCompiler.Instance().RunTask(task).Result;
        if (output.Error) return GenerateError(output);
        var result = ValidateTestCase(task.SingleTestCase(index).Output, output.Results[index]);
        return result;
    }

    private static TestCaseResult GenerateError(CompilerResultsDTO output)
    {
        return new TestCaseResult
        {
            Success = false,
            Error = true,
            Description = FormatOutput(output.Error_message)
        };
    }

    public static TaskResult Submit(GameTask task)
    {
        var outputs = CodeCompiler.Instance().RunTaskValidators(task).Result;
        var testCaseResults = outputs.Results
            .Select((t, i) =>
                ValidateTestCase(task.ValidatorCases[i].Output, t))
            .ToList();
        var success = testCaseResults.All(testCaseResult => testCaseResult.Success);
        return new TaskResult(testCaseResults, success, outputs.Error);
    }

    private static TestCaseResult ValidateTestCase(string expected, TestCaseResult result)
    {
        if (result.Error)
        {
            result.Description = FormatOutput(result.Description);
            return result;
        }

        var output = result.Description;
        if (expected == output)
        {
            result.Success = true;
            result.Description = expected;
            return result;
        }

        result.Description = $"You got {output}, expected {expected}";
        return result;
    }

    private static string FormatOutput(string output)
    {
        const string internalTimeoutMessage = "timelimit: sending warning signal 15";
        const string clientTimeoutMessage = "Timeout! Execution timed out after 3 seconds.\n Make your code faster :)";
        if (output.Contains(internalTimeoutMessage))
        {
            output = output.Replace(internalTimeoutMessage, clientTimeoutMessage);
            return output;
        }

        const int maxLineLength = 20;
        var lines = output.Split("\n");
        if (lines.Length <= maxLineLength)
            return output;
        var builder = new StringBuilder();
        for (var i = 0; i < maxLineLength; i++) builder.Append(lines[i]).AppendLine();
        builder.Append($"{lines.Length - maxLineLength} more lines...");
        return builder.ToString();
    }
}