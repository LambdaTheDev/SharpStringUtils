using System;

namespace LambdaTheDev.SharpStringUtils.Extensions
{
    // Extension methods for Spans (Since .NET Standard 2.1)
    public static class StringSegmentExtensions
    {
#if NETSTANDARD2_1_OR_GREATER

        public static ReadOnlySpan<char> ToSpan(this StringSegment segment)
        {
            ReadOnlySpan<char> span = segment.OriginalString;
            return span.Slice(segment.Offset, segment.Count);
        }
        
#endif
    }
}