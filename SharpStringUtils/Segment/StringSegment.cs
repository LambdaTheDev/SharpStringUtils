using System;

namespace LambdaTheDev.SharpStringUtils.Segment
{
    // Representation of a part of string, used in reusable/non-alloc operations within this library.
    public readonly partial struct StringSegment
    {
        public static readonly StringSegment Null = new StringSegment(null, 0, -1);
        
        public readonly string TargetString;
        public readonly int Offset;
        public readonly int Length;

        public bool IsNull => Length == -1;
        public bool IsEmpty => Length == 0;
        public bool IsNullOrEmpty => Length == -1 || Length == 0;


        // Constructor for string
        public StringSegment(string str)
        {
            if (str == null)
            {
                TargetString = null;
                Offset = 0;
                Length = -1;
            }
            else
            {
                TargetString = str;
                Offset = 0;
                Length = str.Length;
            }
        }

        // Constructor for string with fixed offset & length
        public StringSegment(string str, int offset, int length)
        {
            if (str == null)
            {
                TargetString = null;
                Offset = 0;
                Length = -1;
                return;
            }

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be a positive number!");

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive number!");

            int inArrayCheck = str.Length - offset;
            if (length > inArrayCheck)
                throw new IndexOutOfRangeException("String offset and length must be inside string bounds!");

            TargetString = str;
            Offset = offset;
            Length = length;
        }
        
        // Implicit conversion string => StringSegment
        public static implicit operator StringSegment(string str) => new StringSegment(str);
    }
}