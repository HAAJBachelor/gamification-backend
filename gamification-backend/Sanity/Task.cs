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

    public Reward[] Rewards { get; set; }

    public string Difficulty { get; set; }

    public string Category { get; set; }
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

public class Reward : SanityDocument
{
    public Reward() : base()
    {
    }

    public string Type { get; set; }
    public int Amount { get; set; }
}