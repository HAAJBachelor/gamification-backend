using gamification_backend.Models;
using gamification_backend.Service;
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
    public void Java()
    {
        var value = StubService.GenerateCode(
            "read k:boolean\nloop 5 m:string\nread n:int\nloopline 4 s:string n:int\nwrite 2 2",
            StubGenerator.Language.Java);
        _testOutputHelper.WriteLine(value);
    }

    [Fact]
    public void Csharp()
    {
        var value = StubService.GenerateCode(
            "read k:boolean\nloop 5 m:string\nread n:int\nloopline 4 s:string n:int\nwrite 2 2",
            StubGenerator.Language.Csharp);
        _testOutputHelper.WriteLine(value);
    }

    [Fact]
    public async Task Test()
    {
        for (var i = 0; i < 100; i++)
        {
            var t = new TestCase
            {
                Input = "3",
                Output = "3"
            };
            var tc = new List<TestCase>();
            tc.Add(t);
            var gt = new GameTask
            {
                Language = i % 2 == 0 ? "java" : "csharp",
                UserCode = "class Solution{public static void main(String[] args){System.out.println(2);}}",
                TestCases = tc,
                SessionId = i
            };
            var res = await CodeCompiler.Instance().RunTask(gt);
            if (res.Error)
                _testOutputHelper.WriteLine(res.Error_message);
            else
                _testOutputHelper.WriteLine(res.Results[0].Description);
        }
    }
}