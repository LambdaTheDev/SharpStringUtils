using System;
using System.Text;
using LambdaTheDev.SharpStringUtils.Encodings;
using LambdaTheDev.SharpStringUtils.Extensions;
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

        [Test]
        public void MixedTest()
        {
            string a = "xyzdsamd90u29r8nm8fy8wenfym81ur908m139rdmeqaf";
            string b = "kosjafcm9qe8fm09if90mqe9um980wfu82m89m9i89u9i9";
            char[] array = b.ToCharArray();
            ArraySegment<char> segment = new ArraySegment<char>(array);
            
            _builder.Clear();
            _builder.Append(a, ".");
            _builder.Append(segment, ".");

            string joined = string.Join('.', a, b);
            
            Assert.True(joined == _builder.GetString());
        }
    }
}