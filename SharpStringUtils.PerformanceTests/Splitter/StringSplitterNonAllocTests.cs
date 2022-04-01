using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests.Splitter
{
    [MemoryDiagnoser]
    public class StringSplitterNonAllocTests
    {
        private const string ToSplit = "faifumdsfusds.dif9u9j84r3.fdusanf98um3.fin9asfunas8nfas.afudn8nu8f.sunfd88nqw8nf.fdunq8weuf0q";
        private readonly StringSegment _segment = new StringSegment(ToSplit);
        
        
        // Cannot get rid of 96-byte allocation, probably caused by boxing.
        //  I tried to get rid IEnumerable interface from StringSplitterNonAlloc, but allocation still existed
        [Benchmark]
        public void SplitString()
        {
            foreach (var sgmt in _segment.Split('.'))
            {
                
            }
        }
    }
}