using System.Text;
using LambdaTheDev.SharpStringUtils.Encodings;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.EncodingTests
{
    [TestFixture]
    public class EncodingTests
    {
        private readonly EncodingNonAlloc _encodingNonAlloc = new EncodingNonAlloc(new UTF8Encoding());
        private readonly UTF8Encoding _encodingUtf8 = new UTF8Encoding();

        // Checks if non-alloc byte array matches system array 
        [Test]
        public void ToByteArrayTest()
        {
            string testString = "abcdefg1234";
            byte[] systemBytes = _encodingUtf8.GetBytes(testString);
            byte[] nonAllocBytes = _encodingNonAlloc.GetBytes(testString);
            
            if(systemBytes.Length != nonAllocBytes.Length)
                Assert.Fail();
            
            for(int i = 0; i < systemBytes.Length; i++)
                if(systemBytes[i] != nonAllocBytes[i])
                    Assert.Fail();
            
            Assert.Pass();
        }

        [Test]
        public void EmptyStringTest()
        {
            string testString = "";
            byte[] systemBytes = _encodingUtf8.GetBytes(testString);
            byte[] nonAllocBytes = _encodingNonAlloc.GetBytes(testString);
            
            if(systemBytes.Length != nonAllocBytes.Length)
                Assert.Fail();
            
            for(int i = 0; i < systemBytes.Length; i++)
                if(systemBytes[i] != nonAllocBytes[i])
                    Assert.Fail();
            
            Assert.Pass();
        }
        
        // Check if non-alloc string matches system string
        [Test]
        public void ToStringTest()
        {
            string someString = "abcdefg1234";
            byte[] bytes = _encodingUtf8.GetBytes(someString);

            string checkString = _encodingNonAlloc.GetString(bytes);
            Assert.True(checkString == someString);
        }
    }
}