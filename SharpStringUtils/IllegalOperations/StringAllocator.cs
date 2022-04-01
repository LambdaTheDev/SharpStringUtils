using System;
using System.Reflection;
using LambdaTheDev.SharpStringUtils.Consts;

namespace LambdaTheDev.SharpStringUtils.IllegalOperations
{
    // This static class allows you to manually allocate a string.
    // !!! WARNING !!! - this operation is considered as ILLEGAL in the C# Language.
    //  This may work today, and corrupt your application tomorrow. Use on your own risk.
    [Obsolete(IllegalOperationsMessage.ObsoleteMsg)]
    public static class StringAllocator
    {
        // Pre-allocated array that contains reflection method argument
        private static readonly object[] Argument = new object[1];

        // Method captured by reflection
        private static readonly MethodInfo FastAllocateStringMethod;

        // True, if initialized successfully
        private static readonly bool InitializedSuccessfully;


        // Static cctor that loads a FastAllocateString method
        static StringAllocator()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
            FastAllocateStringMethod = typeof(string).GetMethod("FastAllocateString", flags);
            InitializedSuccessfully = FastAllocateStringMethod != null;
        }

        // This method allocates memory for a string
        public static string Allocate(int length)
        {
            // Check if initialized successfully
            if (!InitializedSuccessfully)
                throw new Exception(
                    "Could not allocate a string, method string.FastAllocateString was undetected/unreachable!");

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive number!");

            // Put argument to object array & invoke reflection method
            Argument[0] = length;
            object reflectionResult = FastAllocateStringMethod.Invoke(null, Argument);

            // Cast to string & return
            return (string)reflectionResult;
        }
    }
}