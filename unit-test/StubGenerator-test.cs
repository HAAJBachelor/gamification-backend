using gamification_backend.Stub;
using Xunit.Abstractions;

namespace unit_test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Stub()
    {
        var value = StubService.GenerateCode(
            "read k:boolean\nloop 5 m:string\nread n:int\nloopline 4 s:string n:int\nwrite 2 2");
        _testOutputHelper.WriteLine(value);
    }
}