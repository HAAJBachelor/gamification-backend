using System.Text;
using System.Text.Json;
using GamificationBackend.Models;

namespace GamificationBackend.Service;

public class CodeCompiler
{
    private static CodeCompiler? _instance;
    private readonly HttpClient? _client;

    private CodeCompiler()
    {
        _client = new HttpClient();
    }

    public static CodeCompiler Instance()
    {
        return _instance ??= new CodeCompiler();
    }

    public async Task<List<string>> RunTask(GameTask task)
    {
        // Serialize our concrete class into a JSON String
        var stringPayload = JsonSerializer.Serialize(task);

        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");


        // Do the actual request and await the response
        var httpResponse = await _client.PostAsync("http://localhost/8000/compiler/", httpContent);

        // If the response contains content we want to read it!
        if (httpResponse.Content != null)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
            var result = JsonSerializer.Deserialize<List<string>>(responseContent);
            return result;
        }

        throw new Exception();
    }
}