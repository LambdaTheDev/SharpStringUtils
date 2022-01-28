using System;
using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Encodings.Base;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests.Tests
{
    [MemoryDiagnoser]
    public class Base64PerformanceTests
    {
        private readonly Base64EncoderNonAlloc _nonAllocEncoder = new Base64EncoderNonAlloc('+', '/', true);

        private byte[][] _bytesToEncode;
        private string[] _stringsToDecode;
        private char[] _charArray = new char[100000];

        [GlobalSetup]
        public void Setup()
        {
            Random rng = new Random();
            _bytesToEncode = new byte[10][];
            _stringsToDecode = new string[10];
            
            for (int i = 0; i < 10; i++)
            {
                byte[] randomBytes = new byte[rng.Next(100, 10000)];
                rng.NextBytes(randomBytes);
                _bytesToEncode[i] = randomBytes;

                _stringsToDecode[i] = Convert.ToBase64String(_bytesToEncode[i]);
            }
        }

        [Benchmark]
        public void Base64EncodingNonAllocBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                ArraySegment<char> base64Chars =
                    _nonAllocEncoder.ToBaseNonAlloc(new ArraySegment<byte>(_bytesToEncode[i]));
            }
        }

        [Benchmark]
        public void Base64EncodingSystemBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                Convert.ToBase64CharArray(_bytesToEncode[i], 0, _bytesToEncode[i].Length, _charArray, 0);
            }
        }
        
        [Benchmark]
        public void Base64DecodingNonAllocBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                ArraySegment<byte> decodedBytes = _nonAllocEncoder.FromBaseNonAlloc(_stringsToDecode[i]);
            }
        }

        [Benchmark]
        public void Base64DecodingSystemBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                byte[] decodedBytes = Convert.FromBase64String(_stringsToDecode[i]);
            }
        }
    }
}