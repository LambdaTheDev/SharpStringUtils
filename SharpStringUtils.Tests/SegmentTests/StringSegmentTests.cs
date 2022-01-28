using System;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.SegmentTests
{
    [TestFixture]
    public class StringSegmentTests
    {
        private readonly string _testStr = "abcdefghijklmnop";
        

        // Ensures that sliced strings are the same, just by cutting from specific offset
        [Test]
        public void SlicingTestA()
        {
            StringSegment segment = new StringSegment(_testStr);
            string slicedSegment = segment.Slice(6).ToString();
            string slicedString = _testStr.Substring(6);
            
            Assert.True(slicedSegment == slicedString);
        }

        // Ensures that sliced strings are the same by cutting from offset to specific length
        [Test]
        public void SlicingTestB()
        {
            StringSegment segment = new StringSegment(_testStr);
            string slicedSegment = segment.Slice(3, 5).ToString();
            string slicedString = _testStr.Substring(3, 5);
            
            Console.WriteLine(slicedSegment + "\n" + slicedString);
            
            Assert.True(slicedSegment == slicedString);
        }

        // Ensures that .ToString() method works as intended
        [Test]
        public void SlicingTestC()
        {
            StringSegment segment = new StringSegment(_testStr);
            string miniSlice = segment.Slice(1).ToString();
            string stringMiniSlice = _testStr.Substring(1);
            
            Assert.True(miniSlice == stringMiniSlice);
        }

        // Ensures that .ToString() method works correctly for non-modified string 
        [Test]
        public void SlicingTestD()
        {
            StringSegment segment = new StringSegment(_testStr);
            Assert.True(segment.ToString() == _testStr);
        }
    }
}