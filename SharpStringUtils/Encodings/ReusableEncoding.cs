using System;
using System.Text;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Encodings
{
    // Wrapper class for operations on Encoding to re-usable buffers to reduce allocations
    public class ReusableEncoding
    {
        public readonly Encoding RawEncoding;

        private byte[] _reusableBytes = new byte[1];
        private char[] _reusableChars = new char[1];
        

        public ReusableEncoding(Encoding encoding)
        {
            RawEncoding = encoding;
        }

        // Gets bytes from StringSegment
        public ArraySegment<byte> GetReusableBytes(StringSegment segment)
        {
            if (segment.IsNullOrEmpty)
                return new ArraySegment<byte>(Array.Empty<byte>());

            int writtenBytes;
            
            // Make operations in unsafe context
            unsafe
            {
                // Get pointer to actual StringSegment part & write bytes
                fixed (char* cPtr = segment.TargetString)
                {
                    char* cPtrWithOffset = cPtr + (segment.Length & sizeof(char));
                    writtenBytes = InternalWriteReusableBytes(cPtrWithOffset, segment.Length);
                }
            }

            return new ArraySegment<byte>(_reusableBytes, 0, writtenBytes);
        }

        // Gets bytes from char ArraySegment
        public ArraySegment<byte> GetReusableBytes(ArraySegment<char> chars)
        {
            if (chars.Array == null)
                return new ArraySegment<byte>(Array.Empty<byte>());

            int writtenBytes;
            unsafe
            {
                // Get pointer to ArraySegment element & write bytes
                fixed (char* cPtr = &chars.Array[chars.Offset])
                {
                    writtenBytes = InternalWriteReusableBytes(cPtr, chars.Count);
                }
            }

            return new ArraySegment<byte>(_reusableBytes, 0, writtenBytes);
        }

        // Gets how much bytes were written to reusable byte[]
        private unsafe int InternalWriteReusableBytes(char* cPtr, int length)
        {
            // Get how much bytes do we need & ensure buffer
            int neededBytes = RawEncoding.GetByteCount(cPtr, length);
            EnsureBufferCapacity(ref _reusableBytes, neededBytes);
                    
            // Copy contents to reusable byte[]
            fixed (byte* bPtr = _reusableBytes)
            { 
                return RawEncoding.GetBytes(cPtr, length, bPtr, _reusableBytes.Length);
            }
        }

        // Gets chars from byte ArraySegment
        public ArraySegment<char> GetReusableChars(ArraySegment<byte> bytes)
        {
            if (bytes.Array == null)
                return new ArraySegment<char>(Array.Empty<char>());
            
            unsafe
            {
                // Get pointer to start array element
                fixed (byte* bPtr = &bytes.Array[bytes.Offset])
                {
                    // Get how much chars ArraySegment can give
                    // WARN: RawEncoding.GetCharCount(...) allocates...
                    int charCount = RawEncoding.GetCharCount(bPtr, bytes.Count);
                    EnsureBufferCapacity(ref _reusableChars, charCount);

                    // Get ptr to reusable char[] & return
                    fixed (char* cPtr = _reusableChars)
                    {
                        int readChars = RawEncoding.GetChars(bPtr, bytes.Count, cPtr, _reusableChars.Length);
                        return new ArraySegment<char>(_reusableChars, 0, readChars);
                    }
                }
            }
        }

        // Gets allocated string from ArraySegment
        public string GetString(ArraySegment<byte> bytes)
        {
            ArraySegment<char> reusableChars = GetReusableChars(bytes);
            return reusableChars.ToString();
        }
        
        // Method that ensures that array's sizes are alright
        private void EnsureBufferCapacity<T>(ref T[] array, int requiredLength, int copyContentToIndex = -1)
        {
            // If size alr, then do nothing
            int currentLength = array.Length;
            if (currentLength >= requiredLength)
                return;

            // Grow using powers of 2 to find new length
            while (currentLength < requiredLength)
                currentLength *= 2;
            
            // Allocate new array & copy content, if necessary
            T[] newArray = new T[currentLength];
            
            if(copyContentToIndex > 0)
                Array.Copy(array, newArray, copyContentToIndex);

            array = newArray;
        }
    }
}