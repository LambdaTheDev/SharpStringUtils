using System;
using BenchmarkDotNet.Running;
using LambdaTheDev.SharpStringUtils.Encodings;

namespace SharpStringUtils.PerformanceTests
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Base64PerfTest>();
            // var summary = BenchmarkRunner.Run<StringSplitterPerfTests>();
            // BenchmarkRunner.Run<EncodingNonAllocPerfTest>();
        }
    }
}