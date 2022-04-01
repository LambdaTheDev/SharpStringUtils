using System;

namespace LambdaTheDev.SharpStringUtils.Segment
{
    // Equality implementations for StringSegment.
    // Equal StringSegment is a StringSegment that contains the same .ToString() value (has equal characters)
    public partial struct StringSegment : IEquatable<StringSegment>, IEquatable<string>
    {
        public bool Equals(StringSegment other)
        {
            if (Length != other.Length)
                return false;

            for (int i = 0; i < Length; i++)
            {
                if (TargetString[Offset + i] != other.TargetString[other.Offset + i])
                    return false;
            }

            return true;
        }

        public bool Equals(string other)
        {
            if (other == null)
                return false;

            StringSegment segment = new StringSegment(other);
            return Equals(segment);
        }

        public override bool Equals(object obj)
        {
            return obj is StringSegment other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TargetString != null ? TargetString.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Offset;
                hashCode = (hashCode * 397) ^ Length;
                return hashCode;
            }
        }

        public static bool operator ==(StringSegment left, StringSegment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringSegment left, StringSegment right)
        {
            return !left.Equals(right);
        }
    }
}