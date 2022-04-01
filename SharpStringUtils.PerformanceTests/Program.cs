using BenchmarkDotNet.Running;
using LambdaTheDev.SharpStringUtils.PerformanceTests.Splitter;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests
{
    public static class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<StringSplitterNonAllocTests>();
        }
    }
}