using Sanity.Linq.CommonTypes;

namespace gamification_backend.Sanity;

public class Task : SanityDocument
{
    public Task() : base()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public Case[] TestCases { get; set; }
    public string Stub { get; set; }
}

public class Case : SanityDocument
{
    public Case() : base()
    {
    }

    public string TestCaseInput { get; set; }
    public string TestCaseOutput { get; set; }
    public string ValidatorInput { get; set; }
    public string ValidatorOutput { get; set; }
}