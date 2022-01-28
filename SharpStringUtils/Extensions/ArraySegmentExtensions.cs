using System;

namespace LambdaTheDev.SharpStringUtils.Extensions
{
    // Extension methods for ArraySegment
    public static class ArraySegmentExtensions
    {
        // Platform-safe .ToArray() implementation for ArrSegmt
        public static T[] SafeToArray<T>(this ArraySegment<T> segment)
        {
#if NETSTANDARD2_1_OR_GREATER
            return segment.ToArray();
#else
            if (segment.Array == null)
                return null;

            if (segment.Count == 0)
                return Array.Empty<T>();
            
            T[] array = new T[segment.Count];
            Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);
            return array;
#endif
        }
    }
}