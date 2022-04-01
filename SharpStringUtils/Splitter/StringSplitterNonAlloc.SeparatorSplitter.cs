using System;
using System.Collections;
using LambdaTheDev.SharpStringUtils.Exceptions;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Splitter
{
    public partial struct StringSplitterNonAlloc
    {
        private class SeparatorSplitter : ISplitter
        {
            private StringSegment _target;
            private Func<char, bool> _validator;

            private StringSegment _current;
            private int _position;
            private bool _ended;

            public char Separator { get; set; }
            public StringSegment Current => _current;
            object IEnumerator.Current => Current;

            
            
            public bool MoveNext()
            {
                // Ending condition
                if (_ended)
                    return false;

                // Validation
                if (_target == null)
                {
                    _current = StringSegment.Null;
                    _ended = true;
                    return true;
                }

                if (_target == StringSegment.Empty)
                {
                    _current = StringSegment.Empty;
                    _ended = true;
                    return true;
                }

                // Actual iteration through string
                for (int i = _position; i < _target.Length; i++)
                {
                    if(!_validator.Invoke(_target.TargetString[_target.Offset + i]))
                        ThrowOnInvalidChar(_target.TargetString[i]);
                
                    if (_target.TargetString[i] == Separator)
                    {
                        _current = new StringSegment(_target.TargetString, _position, i - _position);
                        _position = i + 1;

                        return true;
                    }
                }

                // End of string -> wrap everything that it left
                _current = new StringSegment(_target.TargetString, _position, _target.Length - _position);
                _ended = true;

                return true;
            }

            public void Reset()
            {
                _current = default;
                _ended = false;
                _position = 0;
            }
            
            public void Prepare(StringSegment segment, Func<char, bool> validator)
            {
                _target = segment;
                _validator = validator;
            }
            
            public void Dispose()
            {
                SeparatorSplittersPool.Push(this);
            }

            private void ThrowOnInvalidChar(char c)
            {
                throw new InvalidCharacterException(c);
            }
        }
    }
}