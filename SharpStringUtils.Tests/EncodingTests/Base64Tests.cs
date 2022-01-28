using System;
using LambdaTheDev.SharpStringUtils.Encodings.Base;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.EncodingTests
{
    [TestFixture]
    public class Base64Tests
    {
        private readonly Base64EncoderNonAlloc _base64 = new Base64EncoderNonAlloc('+', '/', true);

        [Test]
        public void EncoderTest()
        {
            Random rng = new Random();
            byte[] randomBytes = new byte[100];
            rng.NextBytes(randomBytes);

            string systemBase64 = Convert.ToBase64String(randomBytes);
            string nonAllocBase64 = _base64.ToBase(new ArraySegment<byte>(randomBytes));
            
            Assert.True(systemBase64 == nonAllocBase64);
        }

        [Test]
        public void DecoderTests()
        {
            Random rng = new Random();
            byte[] randomBytes = new byte[100];
            rng.NextBytes(randomBytes);
            string base64String = Convert.ToBase64String(randomBytes);

            byte[] bytesNonAlloc = _base64.FromBase(base64String);
            
            if(bytesNonAlloc.Length != randomBytes.Length)
                Assert.Fail();
            
            for(int i = 0; i < randomBytes.Length; i++)
                if(randomBytes[i] != bytesNonAlloc[i])
                    Assert.Fail();
            
            Assert.Pass();
        }
    }
}