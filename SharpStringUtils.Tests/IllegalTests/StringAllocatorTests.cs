using System;
using LambdaTheDev.SharpStringUtils.IllegalOperations;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.IllegalTests
{
    [TestFixture]
    public class StringAllocatorTests
    {
        // Check if allocated string has expected length, no more
        [Test]
        public void TestAllocator()
        {
            int expectedLength = 10;
            string allocated = StringAllocator.Allocate(expectedLength);
            
            Assert.True(allocated.Length == expectedLength);
        }

        [Test]
        public void TestNegativeLength()
        {
            int negative = -1;
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                StringAllocator.Allocate(negative);
            });
        }

        [Test]
        public void TestZeroLength()
        {
            string allocated = StringAllocator.Allocate(0);
            Assert.True(allocated.Length == 0);
        }
    }
}