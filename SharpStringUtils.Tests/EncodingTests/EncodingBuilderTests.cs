using System;
using System.Text;
using LambdaTheDev.SharpStringUtils.Encodings;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.EncodingTests
{
    [TestFixture]
    public class EncodingBuilderTests
    {
        private readonly EncodingBuilderNonAlloc _builder = new EncodingBuilderNonAlloc(new UTF8Encoding());


        // Check if it just works
        [Test]
        public void BasicTest()
        {
            _builder.Clear();
            _builder.Append(new StringSegment("A"));
            _builder.Append(new StringSegment("B"));
            _builder.Append(new StringSegment("C"));

            string expected = "ABC";
            string output = _builder.GetString();
            
            Assert.True(expected == output);
        }

        [Test]
        public void SeparatorTest()
        {
            _builder.Clear();
            _builder.Append(new StringSegment("A"), ".");
            _builder.Append(new StringSegment("B"), ".");
            _builder.Append(new StringSegment("C"), ".");

            string expected = "A.B.C";
            string output = _builder.GetString();
            
            Assert.True(expected == output);
        }

        [Test]
        public void NullAppendTest()
        {
            _builder.Clear();
            _builder.Append(new StringSegment("A"), ".");
            _builder.Append(new StringSegment(null), ".");
            _builder.Append(new StringSegment("C"), ".");

            string expected = "A.C";
            string output = _builder.GetString();
            
            Assert.True(expected == output);
        }
        
        [Test]
        public void EmptyAppendTest()
        {
            _builder.Clear();
            _builder.Append(new StringSegment("A"), ".");
            _builder.Append(new StringSegment(""), ".");
            _builder.Append(new StringSegment("C"), ".");

            string expected = "A..C";
            string output = _builder.GetString();
            
            Assert.True(expected == output);
        }
    }
}