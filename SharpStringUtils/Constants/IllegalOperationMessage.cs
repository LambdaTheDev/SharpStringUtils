namespace LambdaTheDev.SharpStringUtils.Constants
{
    // This class contains message with a warning while using Illegal string operations 
    public static class IllegalOperationMessage
    {
        public const string ObsoleteMsg = "WARNING - You are doing an operation, which is considered ILLEGAL " +
                                          "in the C# enviornment. Today it may work, and tomorrow it can crash " +
                                          "your application. Use it on your own risk. To get rid of this warning, " +
                                          "feel free to #pragma warning disable it.";
    }
}