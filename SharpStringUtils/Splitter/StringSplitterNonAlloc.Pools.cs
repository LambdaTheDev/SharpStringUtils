using System.Collections.Concurrent;

namespace LambdaTheDev.SharpStringUtils.Splitter
{
    public partial struct StringSplitterNonAlloc
    {
        private static readonly ConcurrentStack<SeparatorSplitter> SeparatorSplittersPool = new ConcurrentStack<SeparatorSplitter>();
        private static readonly ConcurrentStack<PatternSplitter> PatternSplittersPool = new ConcurrentStack<PatternSplitter>();
    }
}