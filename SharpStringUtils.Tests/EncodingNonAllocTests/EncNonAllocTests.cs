using System;
using System.Text;
using LambdaTheDev.SharpStringUtils.Encodings;
using NUnit.Framework;

namespace SharpStringUtils.Tests.EncodingNonAllocTests
{
    [TestFixture]
    public class EncNonAllocTests
    {
        [Test]
        public void BasicTest()
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            EncodingNonAlloc utf8NonAlloc = new EncodingNonAlloc(Encoding.UTF8);

            string x = "ԇΓŔf]񶦟ꞽ󮞰f";
            byte[] normalBytes = utf8.GetBytes(x);
            ArraySegment<byte> nonAllocBytes = utf8NonAlloc.GetBytesNonAlloc(x);
            
            Assert.True(normalBytes.Length == nonAllocBytes.Count);
        }

        [Test]
        public void DecodeTests()
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            EncodingNonAlloc utf8NonAlloc = new EncodingNonAlloc(Encoding.UTF8);

            string x = "ԇΓŔf]񶦟ꞽ󮞰f";
            byte[] normalBytes = utf8.GetBytes(x);
            ArraySegment<byte> nonAllocBytes = utf8NonAlloc.GetBytesNonAlloc(x);

            string normalStr = utf8.GetString(normalBytes);
            string nonAllocStr = utf8NonAlloc.GetString(nonAllocBytes);
            
            Assert.True(normalStr == nonAllocStr);
        }
    }
}