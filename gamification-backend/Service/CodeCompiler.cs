using System.Text;
using System.Text.Json;
using gamification_backend.DTO;
using gamification_backend.Models;

namespace gamification_backend.Service;

public class CodeCompiler
{
    private static CodeCompiler? _instance;
    private readonly HttpClient _client;

    private CodeCompiler()
    {
        _client = new HttpClient();
    }

    public static CodeCompiler Instance()
    {
        return _instance ??= new CodeCompiler();
    }

    public async Task<CompilerResultsDTO> RunTaskValidators(GameTask task)
    {
        var payLoad = DTOMapper.FromGameTaskToCompilerTask(task, -1, true);
        return await RunTaskImpl(payLoad);
    }

    public async Task<CompilerResultsDTO> RunTask(GameTask task, int testcaseIndex = -1)
    {
        var payLoad = DTOMapper.FromGameTaskToCompilerTask(task, testcaseIndex);
        Console.WriteLine(payLoad.Language);
        return await RunTaskImpl(payLoad);
    }

    private async Task<CompilerResultsDTO> RunTaskImpl(CompilerTaskDTO payload)
    {
        var stringPayload = JsonSerializer.Serialize(payload);
        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");


        // Do the actual request and await the response
        var httpResponse =
            await _client.PostAsync("http://oxxcodecompiler.azurewebsites.net/compiler/", httpContent);

        // If the response contains content we want to read it!
        if (httpResponse.Content == null)
            throw new Exception("Empty content from response");

        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);

        // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
        var result = JsonSerializer.Deserialize<CompilerResultsDTO>(responseContent);
        if (result == null)
            throw new Exception("Could not parse response");
        return result;
    }
}