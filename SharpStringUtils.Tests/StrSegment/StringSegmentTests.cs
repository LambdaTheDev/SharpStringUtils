using System;
using LambdaTheDev.SharpStringUtils;
using NUnit.Framework;

namespace SharpStringUtils.Tests.StrSegment
{
    [TestFixture]
    public class StringSegmentTests
    {
        [Test]
        public void WrappingTest()
        {
            string x = "12345";
            StringSegment segment = new StringSegment(x);
            
            Assert.True(segment.ToString() == x);
        }

        [Test]
        public void SlicingTestA()
        {
            string x = "abcdefg123456";
            
            StringSegment segment = new StringSegment(x);
            string subStr = x.Substring(5);
            StringSegment slicedSegment = segment.Slice(5);
            
            Assert.True(subStr == slicedSegment.ToString());
        }
        
        [Test]
        public void SlicingTestB()
        {
            string x = "abcdefg123456";
            
            StringSegment segment = new StringSegment(x);
            string subStr = x.Substring(5, 3);
            StringSegment slicedSegment = segment.Slice(5, 3);
            
            Assert.True(subStr == slicedSegment.ToString());
        }

        [Test]
        public void EqualityTest()
        {
            string x = "abcdef123456";
            
            StringSegment segment = new StringSegment(x, 3, x.Length - 3);
            string sliced = x.Substring(3, x.Length - 3);

            Assert.True(segment.Equals(sliced));
        }
    }
}