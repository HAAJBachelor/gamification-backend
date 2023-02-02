using System.Text;
using System.Text.Json;
using GamificationBackend.Models;

namespace GamificationBackend.Service;

public class CodeCompiler
{
    private static readonly HttpClient Client = new();

    public static async Task<List<string>> RunTask(GameTask task)
    {
        // Serialize our concrete class into a JSON String
        var stringPayload = JsonSerializer.Serialize(task);

        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");


        // Do the actual request and await the response
        var httpResponse = await Client.PostAsync("http://localhost/8000/compiler/", httpContent);

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