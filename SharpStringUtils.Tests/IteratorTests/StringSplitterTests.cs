using System;
using LambdaTheDev.SharpStringUtils.Iterator;
using NUnit.Framework;

namespace LambdaTheDev.SharpStringUtils.Tests.IteratorTests
{
    [TestFixture]
    public class StringSplitterTests
    {
        private readonly string[] _testStrings = new[]
        {
            "aaaaa.bbbbb.ccccc",
            "aaaaa.bbbbb",
            ".aaaaa.bbbbb",
            "aaaaa.bbbbb.",
            "aaaaaa",
            ".",
            "..",
            "...",
            ".aaa.",
            "",
            "     ",
            " ",
        };

        private readonly int[] _expectedParts = new[]
        {
            3,
            2,
            3,
            3,
            1,
            2,
            3,
            4,
            3,
            1,
            1,
            1
        };

        [Test]
        public void BigSplitterTests()
        {
            bool success = true;
            
            // Iterate through all test strings
            for (int i = 0; i < _testStrings.Length; i++)
            {
                // Get string, prepare iterator & system split
                string testString = _testStrings[i];
                StringSplitterNonAlloc splitter = new StringSplitterNonAlloc(testString, '.');
                string[] splitString = testString.Split('.');

                int iterationsCount = 0;
                foreach (StringSegment segment in splitter)
                {
                    // Ensure that system split == segment split; // CHECK A
                    if (splitString[iterationsCount] != segment.ToString())
                    {
                        success = false;
                        Console.WriteLine("String (" + testString + ") failed check A!");
                    }
                    
                    iterationsCount++;
                }

                // Ensure that expected parts match iterations count; // CHECK B
                if (iterationsCount != _expectedParts[i])
                {
                    success = false;
                    Console.WriteLine("String (" + testString + ") failed check B!");
                }
                
                // Ensure that system split parts matches segment iteration count; // CHECK C
                if (iterationsCount != splitString.Length)
                {
                    success = false;
                    Console.WriteLine("String (" + testString + ") failed check C!");
                }
            }
            
            // If success, pass
            Assert.True(success);
        }
    }
}