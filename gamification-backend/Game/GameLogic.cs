using System.Text;
using gamification_backend.DTO;
using gamification_backend.Models;
using gamification_backend.Service;

namespace gamification_backend.Game;

public static class GameLogic
{
    public static TestCaseResult RunTestCase(GameTask task, int index)
    {
        var output = CodeCompiler.Instance().RunTask(task, index).Result;
        if (output.Error) return GenerateError(output);
        var result = ValidateTestCase(task.SingleTestCase(index).Output, output.Results[0]);
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

        var lines = result.Description.Split("\n");
        var expectedLines = expected.Split("\n");
        var builder = new StringBuilder();
        for (var i = 0; i < expected.Length; i++)
        {
            if (i == lines.Length)
            {
                builder.AppendLine($"Wrong answer on line {i + 1}: Missing output, expected {expectedLines[i]}");
                break;
            }

            builder.AppendLine($"> {lines[i]}");
            if (lines[i] != expectedLines[i])
            {
                builder.AppendLine($"Wrong answer on line {i + 1}: You got {lines[i]}, expected {expectedLines[i]}");
                break;
            }
        }

        Console.WriteLine(builder.ToString());
        result.Description = builder.ToString();
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
        output = output.Replace("/tmp/Solutions/Solution0/", "");
        var lines = output.Split("\n");
        if (lines.Length <= maxLineLength)
            return output;
        var builder = new StringBuilder();
        for (var i = 0; i < maxLineLength; i++) builder.Append(lines[i]).AppendLine();
        builder.Append($"{lines.Length - maxLineLength} more lines...");
        return builder.ToString();
    }
}