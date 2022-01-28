using BenchmarkDotNet.Running;
using LambdaTheDev.SharpStringUtils.PerformanceTests.Tests;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            // BenchmarkRunner.Run<Base64PerformanceTests>();
            // BenchmarkRunner.Run<EncodingPerformanceTests>();
            BenchmarkRunner.Run<StringSplitterPerformanceTests>();
        }
    }
}