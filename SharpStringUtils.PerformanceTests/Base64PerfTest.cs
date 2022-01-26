using System;
using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Encodings.Base;

namespace SharpStringUtils.PerformanceTests
{
    [MemoryDiagnoser()]
    public class Base64PerfTest
    {
        private readonly string[] _encodedBytes = new[]
        {
            "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",
        };

        private byte[][] _bytesToEncode;
        private readonly Base64EncoderNonAlloc _encoderNonAlloc = new Base64EncoderNonAlloc('+', '/', true);

        [GlobalSetup]
        public void Setup()
        {
            _bytesToEncode = new byte[_encodedBytes.Length][];
            for (int i = 0; i < _encodedBytes.Length; i++)
            {
                _bytesToEncode[i] = Convert.FromBase64String(_encodedBytes[i]);
            }
        }
        
        [Benchmark]
        public void BenchmarkSystemEncodeBase()
        {
            for (int i = 0; i < _bytesToEncode.Length; i++)
            {
                string x = Convert.ToBase64String(_bytesToEncode[i]);
            }
        }

        [Benchmark]
        public void BenchmarkNonAllocEncodeBase()
        {
            for (int i = 0; i < _bytesToEncode.Length; i++)
            {
                ArraySegment<char> x = _encoderNonAlloc.ToBaseNonAlloc(new ArraySegment<byte>(_bytesToEncode[i]));
            }
        }

        [Benchmark]
        public void BenchmarkSystemDecodeBase()
        {
            for (int i = 0; i < _encodedBytes.Length; i++)
            {
                byte[] x = Convert.FromBase64String(_encodedBytes[i]);
            }
        }

        [Benchmark]
        public void BenchmarkNonAllocDecodeBase()
        {
            for (int i = 0; i < _encodedBytes.Length; i++)
            {
                ArraySegment<byte> x = _encoderNonAlloc.FromBaseNonAlloc(_encodedBytes[i]);
            }
        }
    }
}