using System;
using LambdaTheDev.SharpStringUtils.Iterator;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.IteratorTests
{
    [TestFixture]
    public class StringSplitterWithValidatorTests
    {
        private readonly string[] _testStrings = new[]
        {
            "aaaaaaaaa",
            "aaaaaaAaaaaa",
            "A",
            "aaaaa.aaaaa.aaaaa",
            "aaaaa.aaaaa.aaaaA"
        };

        private readonly bool[] _isValid = new[]
        {
            true, false, false, true, false,
        };
        
        private static bool OnlyLowercaseChars(char c)
        {
            return char.IsLower(c) || c == '.';
        }

        [Test]
        public void SplitterWithValidationTests()
        {
            bool success = true;
            
            // Get test strings
            for (int i = 0; i < _testStrings.Length; i++)
            {
                // Wrap them in iterator
                string testString = _testStrings[i];
                StringSplitterNonAlloc splitter = new StringSplitterNonAlloc(testString, '.', OnlyLowercaseChars);

                try
                {
                    // Iterate
                    foreach (StringSegment segment in splitter) { }

                    // If iteration has not thrown, and it's invalid - then test failed
                    if (!_isValid[i])
                    {
                        Console.WriteLine("String " + testString + " failed check A!");
                        success = false;
                    }
                }
                catch (ArgumentException)
                {
                    // If iteration thrown, and is invalid - then test failed
                    if (_isValid[i])
                    {
                        Console.WriteLine("String " + testString + " failed check B!");
                        success = false;
                    }
                }
            }
            
            Assert.True(success);
        }
    }
}