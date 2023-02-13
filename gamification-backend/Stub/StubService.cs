namespace gamification_backend.Stub;

public static class StubService
{
    public static string GenerateCode(string stub, StubGenerator.Language language)
    {
        StubParser parser = new();
        try
        {
            parser.Parse(stub);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var code = StubGenerator.GenerateCode(language, parser);
        return code;
    }
}