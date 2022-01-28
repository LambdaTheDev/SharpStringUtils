using System;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpStringUtils
{
    // An ArraySegment/Span equivalent for this library (especially .NET Standard 2.0 where there is no Spans)
    //  used in non-alloc iterations etc
    public readonly struct StringSegment
    {
        public static readonly StringSegment Null = new StringSegment(null, 0, -1);
        public static readonly StringSegment Empty = new StringSegment(string.Empty, -1, 0);
        
        public readonly string OriginalString; // Reference to original string
        public readonly int Offset; // Offset from original string
        public readonly int Count; // Count of characters

        public bool IsNull => Count == -1;
        public bool IsEmpty => Offset == -1;
        public bool IsNullOrEmpty => Offset == -1 || Count == -1;
        

        // Constructor for raw string
        public StringSegment(string str)
        {
            OriginalString = str;

            if (str == null)
            {
                Offset = 0;
                Count = -1;
            }
            else if (string.IsNullOrEmpty(str))
            {
                Offset = -1;
                Count = 0;
            }
            else
            {
                Offset = 0;
                Count = str.Length;
            }
        }

        // Constructor for string with offset & count
        public StringSegment(string str, int offset, int count)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if(offset < 0)
                    throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be a positive number!");
            
                if(count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count), "Count must be a positive number!");
                
                if(str.Length - offset < count)
                    throw new ArgumentException("Offset and length must be in string bounds!");
            }
            
            OriginalString = str;
            Offset = offset;
            Count = count;

            if (str == null)
                Count = -1;
            
            else if (string.IsNullOrEmpty(str))
                Offset = -1;
        }

        public StringSegment Slice(int offset, int count = -1)
        {
            if(offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be a positive number!");
            
            if(count < -1)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be a positive number, or -1!");

            if (count == -1)
                count = OriginalString.Length - Offset - offset;
            
            return new StringSegment(OriginalString, Offset + offset, count);
        }
        
        #region Operators & system overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return IsNull;

            if (obj is string str)
            {
                return EqualsStringSegment(new StringSegment(str));
            }

            if (obj is StringSegment segment)
            {
                return EqualsStringSegment(segment);
            }

            return false;
        }

        public bool Equals(string str)
        {
            return EqualsStringSegment(new StringSegment(str));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool EqualsStringSegment(StringSegment segment)
        {
            if(Count != segment.Count)
                return false;
            
            for (int i = 0; i < Count; i++)
            {
                if (OriginalString[Offset + i] != segment.OriginalString[segment.Offset + i])
                    return false;
            }

            return true;
        }
        
        public override int GetHashCode()
        {
            return OriginalString.GetHashCode() ^ Offset ^ Count;
        }

        public override string ToString()
        {
            if (Offset == -1)
                return string.Empty;

            if (Count == -1)
                return null;
            
            if (Offset == 0 && Count == OriginalString.Length)
                return OriginalString;

            return OriginalString.Substring(Offset, Count);
        }
        
        public static bool operator ==(StringSegment lhs, StringSegment rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(StringSegment lhs, StringSegment rhs)
        {
            return !(lhs.Equals(rhs));
        }
        
        #endregion
    }
}