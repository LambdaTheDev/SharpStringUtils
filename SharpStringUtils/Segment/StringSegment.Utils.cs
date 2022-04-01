using System;

namespace LambdaTheDev.SharpStringUtils.Segment
{
    // Functionality methods for StringSegment
    public partial struct StringSegment
    {
        // Creates new StringSegment, with the same TargetString, but with offset Offset + from, to Length = to
        public StringSegment Slice(int from, int to = -1)
        {
            if (from < 0)
                throw new ArgumentOutOfRangeException(nameof(from), "Argument from must be a positive number!");

            if (to != -1 && to < 0)
                throw new ArgumentOutOfRangeException(nameof(to), "Argument to must be -1, or a positive number!");
            
            // Here I just instantiate StringSegment, in-cctor checks will throw exceptions on invalid args
            int length = to == -1 ? Length - from : to;
            return new StringSegment(TargetString, Offset + from, length);
        }
        
        public override string ToString()
        {
            if (Length == -1)
                return null;
            
            if (Length == 0)
                return string.Empty;

            if (Offset == 0 && Length == TargetString.Length)
                return TargetString;

            return TargetString.Substring(Offset, Length);
        }
    }
}