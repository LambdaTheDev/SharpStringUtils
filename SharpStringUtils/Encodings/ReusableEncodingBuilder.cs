using System;
using System.Text;
using LambdaTheDev.SharpStringUtils.Helpers;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Encodings
{
    public class ReusableEncodingBuilder
    {
        private readonly ReusableEncoding _encoding;

        private char[] _separatorBuffer = new char[4];
        private char[] _inputCharBuffer = new char[64];
        private byte[] _reusableOutputByteBuffer = new byte[1];

        private int _separatorCharsCount;
        private int _insertedCharsCount;
        private int _outputBytesCount;
        
        public Encoding RawEncoding => _encoding.RawEncoding;


        // Method parses separator to char[]
        public ReusableEncodingBuilder SetSeparator(string separator)
        {
            ArrayHelpers.ExpandArrayByPowOfTwo(ref _separatorBuffer, separator.Length);
            
            for (int i = 0; i < separator.Length; i++)
            {
                _separatorBuffer[i] = separator[i];
            }

            return this;
        }
        
        // Method appends StringSegment to char[]
        public ReusableEncodingBuilder Append(StringSegment segment)
        {
            ArrayHelpers.ExpandArrayByPowOfTwo(ref _inputCharBuffer,
                _insertedCharsCount + _separatorCharsCount + segment.Length);
            
            AppendSeparator();
            
            for (int i = 0; i < segment.Length; i++)
            {
                _inputCharBuffer[_insertedCharsCount++] = segment.TargetString[segment.Offset + i];
            }
            return this;
        }

        // Method that appends char ArraySegment
        public ReusableEncodingBuilder Append(ArraySegment<char> chars)
        {
            ArrayHelpers.ExpandArrayByPowOfTwo(ref _inputCharBuffer,
                _insertedCharsCount + _separatorCharsCount + chars.Count);
            
            AppendSeparator();
            
            for (int i = 0; i < chars.Count; i++)
            {
                _inputCharBuffer[_insertedCharsCount++] = chars.Array[chars.Offset + i];
            }
            
            return this;
        }

        // Method that appends separator, if necessary
        private void AppendSeparator()
        {
            if (_insertedCharsCount <= 0) return;
            
            for (int i = 0; i < _separatorCharsCount; i++)
            {
                _inputCharBuffer[_insertedCharsCount++] = _separatorBuffer[i];
            }
        }

        // Method that gets input & gets byte[] from encoding
        public ArraySegment<byte> BuildReusableBytes()
        {
            return _encoding.GetReusableBytes(new ArraySegment<char>(_inputCharBuffer, 0, _insertedCharsCount));
        }

        // Method that resets this EncodingBuilder & clears arrays if user wants so
        public void Clear(bool zeroArrays = false)
        {
            if (zeroArrays)
            {
                ArrayHelpers.Zero(_separatorBuffer, _separatorCharsCount);
                ArrayHelpers.Zero(_inputCharBuffer, _insertedCharsCount);
                ArrayHelpers.Zero(_reusableOutputByteBuffer, _outputBytesCount);
            }

            _separatorCharsCount = 0;
            _insertedCharsCount = 0;
            _outputBytesCount = 0;
        }
    }
}