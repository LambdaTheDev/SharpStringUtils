using System;
using System.Runtime.CompilerServices;
using LambdaTheDev.SharpStringUtils.Encodings;

namespace LambdaTheDev.SharpStringUtils.Extensions
{
    // Extension methods for EncodingNonAlloc, usually QOL ones
    public static class EncodingNonAllocExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this EncodingNonAlloc encoding, StringSegment segment)
        {
            ArraySegment<byte> reusableBytes = encoding.GetBytesNonAlloc(segment);
            return reusableBytes.SafeToArray();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this EncodingNonAlloc encoding, string str)
        {
            ArraySegment<byte> reusableBytes = encoding.GetBytesNonAlloc(new StringSegment(str));
            return reusableBytes.SafeToArray();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<byte> GetBytesNonAlloc(this EncodingNonAlloc encoding, string str)
        {
            return encoding.GetBytesNonAlloc(new StringSegment(str));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<char> GetCharsNonAlloc(this EncodingNonAlloc encoding, byte[] bytes)
        {
            return encoding.GetCharsNonAlloc(new ArraySegment<byte>(bytes));
        }
    }
}