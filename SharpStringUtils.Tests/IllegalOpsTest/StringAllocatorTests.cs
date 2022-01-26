using LambdaTheDev.SharpStringUtils.IllegalOperations;
using NUnit.Framework;

namespace SharpStringUtils.Tests.IllegalOpsTest
{
    [TestFixture]
    public class StringAllocatorTests
    {
        [Test]
        public void BasicTest()
        {
            string x = StringAllocator.Allocate(20);
            Assert.True(x.Length == 20);
        }
    }
}