using System;
using System.Runtime.CompilerServices;
using LambdaTheDev.SharpStringUtils.Encodings;

namespace LambdaTheDev.SharpStringUtils.Extensions
{
    // Extension methods for EncodingBuilderNonAlloc, usually QOL ones
    public static class EncodingBuilderNonAllocExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Append(this EncodingBuilderNonAlloc builder, string str, string separator = null)
        {
            builder.Append(new StringSegment(str), separator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this EncodingBuilderNonAlloc builder)
        {
            ArraySegment<byte> reusableBytes = builder.GetBytesNonAlloc();
            return reusableBytes.SafeToArray();
        }
    }
}