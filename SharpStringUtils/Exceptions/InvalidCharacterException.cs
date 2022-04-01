using System;

namespace LambdaTheDev.SharpStringUtils.Exceptions
{
    // Exception thrown by StringSplitterNonAlloc when it encounters illegal character
    public sealed class InvalidCharacterException : Exception
    {
        public char InvalidChar { get; }
       
        
        public InvalidCharacterException(char c)
        {
            InvalidChar = c;
        }
    }
}