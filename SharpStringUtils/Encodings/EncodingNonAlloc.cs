using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace LambdaTheDev.SharpStringUtils.Encodings
{
    // A wrapper class for encodings use less memory & reuse buffers
    // Note: Maybe add support for Spans?
    public class EncodingNonAlloc
    {
        public readonly Encoding Encoding;

        private byte[] _reusableByteArray;
        private char[] _reusableCharArray;


        public EncodingNonAlloc(Encoding encoding)
        {
            Encoding = encoding;
        }


        // Gets reusable bytes from the string (Note: it releases unmanaged buffer!)
        public ArraySegment<byte> GetBytesNonAlloc(StringSegment segment)
        {
            // If segment is null or empty, then return empty array segment
            if(segment.IsNullOrEmpty)
                return new ArraySegment<byte>(Array.Empty<byte>());
            
            // Get unmanaged bytes & ensure that managed buffer has enough space
            IntPtr unmanagedBuffer = GetUnsafeBytes(segment, out int length);
            EnsureByteBufferCapacity(length);

            // Copy unmanaged buffer to managed & free it
            Marshal.Copy(unmanagedBuffer, _reusableByteArray, 0, length);
            Marshal.FreeHGlobal(unmanagedBuffer);
            
            // Return segment of new buffer
            return new ArraySegment<byte>(_reusableByteArray, 0, length);
        }
        
        // Gets reusable bytes from the chars segment
        public ArraySegment<byte> GetBytesNonAlloc(ArraySegment<char> chars)
        {
            if(chars.Array == null || chars.Count == 0)
                return new ArraySegment<byte>(Array.Empty<byte>());

            int bytes = Encoding.GetBytes(chars.Array, chars.Offset, chars.Count, _reusableByteArray, 0);
            return new ArraySegment<byte>(_reusableByteArray, 0, bytes);
        }

        // Allocates new unmanaged memory & fills it with string content
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe IntPtr GetUnsafeBytes(StringSegment segment, out int length)
        {
            // Get a pointer to a string
            fixed (char* originalStrPtr = segment.OriginalString)
            {
                // Get string's bytes count, allocate buffer, & fill it
                int strByteCount= Encoding.GetByteCount(originalStrPtr + segment.Offset, segment.Count);
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(strByteCount);
                Encoding.GetBytes(originalStrPtr + segment.Offset, segment.Count, (byte*) unmanagedBuffer, strByteCount);

                // Set out value & return ptr
                length = strByteCount;
                return unmanagedBuffer;
            }
        }

        // Frees unmanaged bytes
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FreeUnsafeBytes(IntPtr ptr) => Marshal.FreeHGlobal(ptr);
        

        // Gets chars from bytes into reusable char array
        public ArraySegment<char> GetCharsNonAlloc(ArraySegment<byte> bytes)
        {
            int charCount;
            
            // In unsafe block, get chars & put them in a buffer
            unsafe
            {
                fixed (byte* byteBufferPtr = bytes.Array)
                {
                    // Get char count & ensure buffer size is alright
                    
                    // WARNING: No matter what, _encoding.GetCharCount(...) allocates...
                    charCount = Encoding.GetCharCount(byteBufferPtr + bytes.Offset, bytes.Count);
                    EnsureCharBufferCapacity(charCount);
                    
                    fixed (char* charBufferPtr = _reusableCharArray)
                    {
                        Encoding.GetChars(byteBufferPtr + bytes.Offset, bytes.Count, charBufferPtr,
                            charCount);
                    }
                }
            }
            
            // Wrap char array to segment & return it
            return new ArraySegment<char>(_reusableCharArray, 0, charCount);
        }

        // Gets string from provided bytes. Aggressive inlined, due to it's a method wrapper
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(ArraySegment<byte> bytes)
        {
            // Note: I have not found any better solution for GetString(...)
            return Encoding.GetString(bytes.Array, bytes.Offset, bytes.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureByteBufferCapacity(int requiredLength)
        {
            // Set length variable, set it to current length if != null
            int currentLength = 1;
            if (_reusableByteArray != null)
            {
                // If size is alright, then do nothing
                if (_reusableByteArray.Length >= requiredLength)
                    return;

                currentLength = _reusableByteArray.Length;
            }

            // Iterate through powers of 2 to find a good new size
            while (currentLength < requiredLength)
                currentLength *= 2;
            
            // Update byte array to new one
            // NOTE: No content is being copied. Methods that use that method
            //  will override buffer content, so no need to clear/copy it.
            _reusableByteArray = new byte[currentLength];
        }
        
        // Exactly the same as EnsureByteBuff, but for char buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCharBufferCapacity(int requiredLength)
        {
            int currentLength = 1;
            if (_reusableCharArray != null)
            {
                if (_reusableCharArray.Length >= requiredLength)
                    return;

                currentLength = _reusableCharArray.Length;
            }

            while (currentLength < requiredLength)
                currentLength *= 2;
            
            _reusableCharArray = new char[currentLength];
        }
    }
}