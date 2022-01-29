using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using LambdaTheDev.SharpStringUtils.Extensions;

namespace LambdaTheDev.SharpStringUtils.Encodings
{
    // StringBuilder, but for chosen encoding
    public class EncodingBuilderNonAlloc
    {
        private const int OutputBuffersInitialSize = 32;
        
        
        // Wrapped EncodingNonAlloc
        private readonly EncodingNonAlloc _encoding;

        // Output byte buffer for result
        private byte[] _outputByteBuffer = new byte[OutputBuffersInitialSize];

        // Output byte buffer content length
        private int _appendedBytes;

        // Container for separator content
        private byte[] _separatorBuffer = new byte[4];
        
        
        public EncodingBuilderNonAlloc(Encoding encoding)
        {
            _encoding = new EncodingNonAlloc(encoding);
        }

        // Sets raw data to output buffer.
        // NOTE: This method overrides all content & copies data from ArraySegment to output buffer
        public void SetRawData(ArraySegment<byte> data)
        {
            // Reset & ensure capacity
            Clear();
            EnsureArrayCapacity(ref _outputByteBuffer, data.Count, 0);
            
            // And copy content from ArraySegment
            Buffer.BlockCopy(data.Array, data.Offset, _outputByteBuffer, 0, data.Count);
        }

        // This methods causes encoding errors. For now, less-optimized string one should be fine
        // // Writes separator into buffer & appends StringSegment to rest
        // public void Append(StringSegment segment, char separator)
        // {
        //     // Separator buffer is ALWAYS > 2, so I can just put separator into buffer
        //     _separatorBuffer[0] = (byte) separator;
        //     _separatorBuffer[1] = (byte) (separator >> 8);
        //     InternalAppend(segment, 2); // 2, due to sizeof(char) == 2!!!
        // }

        // Writes separator into buffer & appends StringSegment to rest
        public void Append(StringSegment segment, string separator = null)
        {
            int separatorLength = ParseSeparator(separator);
            InternalAppend(segment, separatorLength);
        }
        
        // Appends chars segment to buffer.
        // todo: Allow StringSegment to accept ArraySegment of chars, string, & pointers
        public void Append(ArraySegment<char> chars, string separator = null)
        {
            // Get length of separator
            int separatorLength = ParseSeparator(separator);
            
            // If array == null, then return
            if (chars.Array == null)
                return;

            int appendedBytes = 0;
            // If needed, append separator
            if (_appendedBytes > 0 && separatorLength > 0)
            {
                // Append separator using loop
                for (int i = 0; i < separatorLength; i++)
                    _outputByteBuffer[_appendedBytes + i] = _separatorBuffer[i];
                
                appendedBytes += separatorLength;
            }

            // Get content bytes & ensure output buffer
            ArraySegment<byte> contentBytes = _encoding.GetBytesNonAlloc(chars);
            EnsureArrayCapacity(ref _outputByteBuffer, _appendedBytes + contentBytes.Count + appendedBytes, _appendedBytes + appendedBytes);
            
            // Append actual content in an unsafe way
            unsafe
            {
                fixed (byte* outputPtr = &_outputByteBuffer[_appendedBytes + appendedBytes])
                fixed (byte* contentPtr = contentBytes.Array)
                {
                    Buffer.MemoryCopy(contentPtr, outputPtr, _outputByteBuffer.Length, contentBytes.Count);
                    appendedBytes += contentBytes.Count;
                }
            }
            
            // Update global appended bytes
            _appendedBytes += appendedBytes;
        }

        // Performs actual string segment appending
        private void InternalAppend(StringSegment content, int separatorByteLength)
        {
            // If content is null, then disregard
            // Note: Empty string should append only separator, if there is one!
            if (content.IsNull)
                return;
            
            // Log how much bytes are appended & append separator if needed
            int appendedBytes = 0;

            // If needed, append separator
            if (_appendedBytes > 0 && separatorByteLength > 0)
            {
                // Append separator using loop
                for (int i = 0; i < separatorByteLength; i++)
                    _outputByteBuffer[_appendedBytes + i] = _separatorBuffer[i];
                
                appendedBytes += separatorByteLength;
            }
            
            // Get StringSegment bytes & ensure output capacity
            ArraySegment<byte> contentBytes = _encoding.GetBytesNonAlloc(content);
            EnsureArrayCapacity(ref _outputByteBuffer, _appendedBytes + contentBytes.Count + appendedBytes, _appendedBytes + appendedBytes);

            // Append actual content in an unsafe way
            unsafe
            {
                fixed (byte* outputPtr = &_outputByteBuffer[_appendedBytes + appendedBytes])
                fixed (byte* contentPtr = contentBytes.Array)
                {
                    Buffer.MemoryCopy(contentPtr, outputPtr, _outputByteBuffer.Length, contentBytes.Count);
                    appendedBytes += contentBytes.Count;
                }
            }
            
            // Update global appended bytes
            _appendedBytes += appendedBytes;
        }

        // Returns how much bytes does separator take
        private int ParseSeparator(string separator)
        {
            // Get separator bytes length & append it
            int separatorLength = separator?.Length ?? 0;
            if (separatorLength > 0)
            {
                // If separator length > 0, then ensure capacity & write separator
                ArraySegment<byte> separatorBytes = _encoding.GetBytesNonAlloc(new StringSegment(separator));
                EnsureArrayCapacity(ref _separatorBuffer, separatorBytes.Count, 0);
                
                // Separator is rarely a high number, so Ill just for(int i...) it...
                for (int i = 0; i < separatorBytes.Count; i++)
                    _separatorBuffer[i] = separatorBytes.Array[i];
                
                // Set separator length to byte length
                separatorLength = separatorBytes.Count;
            }
            
            return separatorLength;
        }

        // Resets position & allows for new usage
        public void Clear()
        {
            _appendedBytes = 0;
        }

        // Gets reusable output
        public ArraySegment<byte> GetBytesNonAlloc()
        {
            return new ArraySegment<byte>(_outputByteBuffer, 0, _appendedBytes);
        }

        // Gets allocated string
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString()
        {
            return _encoding.Encoding.GetString(_outputByteBuffer, 0, _appendedBytes);
        }

        // Ensures array capacity & copies content, if it needs to be extended,
        //  because in this utility buffers aren't always overriden.
        private void EnsureArrayCapacity<T>(ref T[] array, int requiredLength, int arrayCapacity) where T : unmanaged
        {
            int newLength = 1;
            
            if (array != null)
                newLength = array.Length;

            // If capacity is alright, return
            if (newLength > requiredLength)
                return;

            // Use powers of two
            while (newLength < requiredLength)
                newLength *= 2;
            
            // Create new array & copy content in an unsafe way
            T[] newArray = new T[newLength];
            unsafe
            {
                fixed (T* oldArrayPtr = array)
                fixed(T* newArrayPtr = newArray)
                {
                    Buffer.MemoryCopy(oldArrayPtr, newArrayPtr, newLength, arrayCapacity);
                }
            }
            
            // Replace former array with new one
            array = newArray;
        }
    }
}