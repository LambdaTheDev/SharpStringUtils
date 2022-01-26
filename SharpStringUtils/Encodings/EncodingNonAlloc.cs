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
        private readonly Encoding _encoding;

        private byte[] _reusableByteArray;
        private char[] _reusableCharArray;


        public EncodingNonAlloc(Encoding encoding)
        {
            _encoding = encoding;
        }


        // Gets reusable bytes from the string (Note: it releases unmanaged buffer!)
        public ArraySegment<byte> GetBytesNonAlloc(StringSegment segment)
        {
            // Get unmanaged bytes & ensure that managed buffer has enough space
            IntPtr unmanagedBuffer = GetUnsafeBytes(segment, out int length);
            EnsureByteBufferCapacity(length);

            // Copy unmanaged buffer to managed & free it
            Marshal.Copy(unmanagedBuffer, _reusableByteArray, 0, length);
            Marshal.FreeHGlobal(unmanagedBuffer);
            
            // Return segment of new buffer
            return new ArraySegment<byte>(_reusableByteArray, 0, length);
        }

        // Allocates new unmanaged memory & fills it with string content
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe IntPtr GetUnsafeBytes(StringSegment segment, out int length)
        {
            // Get a pointer to a string
            fixed (char* originalStrPtr = segment.OriginalString)
            {
                // Get string's bytes count, allocate buffer, & fill it
                int strByteCount= _encoding.GetByteCount(originalStrPtr + segment.Offset, segment.Count);
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(strByteCount);
                _encoding.GetBytes(originalStrPtr + segment.Offset, segment.Count, (byte*) unmanagedBuffer, strByteCount);

                // Set out value & return ptr
                length = strByteCount;
                return unmanagedBuffer;
            }
        }
        
        public ArraySegment<byte> GetBytesNonAlloc(string text) => GetBytesNonAlloc(new StringSegment(text));


        // Gets chars from bytes into reusable char array
        public ArraySegment<char> GetCharsNonAlloc(ArraySegment<byte> bytes)
        {
            // Get char count & ensure buffer size is alright
            int charCount = _encoding.GetCharCount(bytes.Array, bytes.Offset, bytes.Count);
            EnsureCharBufferCapacity(charCount);

            // In unsafe block, get chars & put them in a buffer
            unsafe
            {
                fixed (byte* byteBufferPtr = bytes.Array)
                fixed (char* charBufferPtr = _reusableCharArray)
                {
                    _encoding.GetChars(byteBufferPtr + bytes.Offset, bytes.Count, charBufferPtr,
                        _reusableCharArray.Length);
                }
            }
            
            // Wrap char array to segment & return it
            return new ArraySegment<char>(_reusableCharArray, 0, charCount);
        }

        public ArraySegment<char> GetCharsNonAlloc(byte[] bytes) => GetCharsNonAlloc(new ArraySegment<byte>(bytes));


        // Gets string from provided bytes
        public string GetString(ArraySegment<byte> bytes)
        {
            // Note: I have not found any better solution for GetString(...)
            return _encoding.GetString(bytes.Array, bytes.Offset, bytes.Count);
        }

        public string GetString(byte[] bytes) => GetString(new ArraySegment<byte>(bytes));
        

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