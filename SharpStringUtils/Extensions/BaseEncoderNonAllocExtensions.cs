using System;
using System.Runtime.CompilerServices;
using LambdaTheDev.SharpStringUtils.Encodings.Base;

namespace LambdaTheDev.SharpStringUtils.Extensions
{
    // Extension methods for BaseEncoderNonAlloc, usually QOL ones
    public static class BaseEncoderNonAllocExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBaseString(this BaseEncoderNonAlloc encoder, ArraySegment<byte> data)
        {
            ArraySegment<char> chars = encoder.ToBaseNonAlloc(data);
            return new string(chars.Array, chars.Offset, chars.Count);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBaseString(this BaseEncoderNonAlloc encoder, byte[] data)
        {
            ArraySegment<char> chars = encoder.ToBaseNonAlloc(new ArraySegment<byte>(data));
            return new string(chars.Array, chars.Offset, chars.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<char> ToBaseNonAlloc(this BaseEncoderNonAlloc encoder, byte[] data)
        {
            return encoder.ToBaseNonAlloc(new ArraySegment<byte>(data));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<byte> FromBaseNonAlloc(this BaseEncoderNonAlloc encoder, string data)
        {
            return encoder.FromBaseNonAlloc(new StringSegment(data));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] FromBase(this BaseEncoderNonAlloc encoder, string data)
        {
            ArraySegment<byte> reusableBytes = encoder.FromBaseNonAlloc(new StringSegment(data));
            return reusableBytes.SafeToArray();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] FromBase(this BaseEncoderNonAlloc encoder, StringSegment segment)
        {
            ArraySegment<byte> reusableBytes = encoder.FromBaseNonAlloc(segment);
            return reusableBytes.SafeToArray();
        }
    }
}