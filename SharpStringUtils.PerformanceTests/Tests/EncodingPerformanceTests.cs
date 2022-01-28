using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Encodings;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests.Tests
{
    [MemoryDiagnoser]
    public class EncodingPerformanceTests
    {
        private readonly UTF8Encoding _system = new UTF8Encoding();
        private readonly EncodingNonAlloc _nonAlloc = new EncodingNonAlloc(new UTF8Encoding());

        private string[] _stringsToDecode;
        private byte[][] _bytesToEncode;
        
        
        [GlobalSetup]
        public void Setup()
        {
            Random rng = new Random();
            _stringsToDecode = new string[10];
            _bytesToEncode = new byte[10][];

            for (int i = 0; i < 10; i++)
            {
                byte[] randomBytes = new byte[rng.Next(100, 10000)];
                rng.NextBytes(randomBytes);

                _stringsToDecode[i] = Convert.ToBase64String(randomBytes);
                rng.NextBytes(randomBytes);
                
                _bytesToEncode[i] = randomBytes;
            }
        }
        

        // [Benchmark]
        public void EncodingNonAllocBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                ArraySegment<byte> encodedBytes = _nonAlloc.GetBytesNonAlloc(new StringSegment(_stringsToDecode[i]));
            }
        }

        // [Benchmark]
        public void EncodingSystemBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                byte[] encodedBytes = _system.GetBytes(_stringsToDecode[i]);
            }
        }
        
        [Benchmark]
        public void DecodingNonAllocBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                ArraySegment<char> decodedChars = _nonAlloc.GetCharsNonAlloc(new ArraySegment<byte>(_bytesToEncode[i]));
            }
        }

        // [Benchmark]
        public void DecodingSystemBenchmark()
        {
            for (int i = 0; i < 10; i++)
            {
                char[] decodedString = _system.GetChars(_bytesToEncode[i]);
            }
        }
    }
}