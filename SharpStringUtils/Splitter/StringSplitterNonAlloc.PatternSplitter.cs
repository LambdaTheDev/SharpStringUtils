using System;
using System.Collections;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Splitter
{
    public partial struct StringSplitterNonAlloc
    {
        private class PatternSplitter : ISplitter
        {
            public StringSegment Current { get; }
            object IEnumerator.Current => Current;

            
            
            public bool MoveNext()
            {
                throw new System.NotImplementedException();
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }
            
            public void Prepare(StringSegment segment, Func<char, bool> validator)
            {
                throw new System.NotImplementedException();
            }

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}