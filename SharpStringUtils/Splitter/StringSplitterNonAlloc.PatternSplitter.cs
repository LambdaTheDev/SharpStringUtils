using System;
using System.Collections;
using LambdaTheDev.SharpStringUtils.Exceptions;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Splitter
{
    public partial struct StringSplitterNonAlloc
    {
        private class PatternSplitter : ISplitter
        {
            private StringSegment _target;
            private Func<char, bool> _validator;

            private StringSegment _current;
            private string _pattern;
            private int _position;
            private bool _ended;

            // Used backing field to avoid calling get & set methods
            public string Pattern
            {
                get => _pattern;
                set => _pattern = value;
            }
            public StringSegment Current => _current;
            object IEnumerator.Current => Current;

            
            
            public bool MoveNext()
            {
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
                
                // Iteration
                for (int i = _position; i < _target.Length; i++)
                {
                    // Validate char in validator
                    if(!_validator.Invoke(_target.TargetString[_target.Offset + i]))
                        ThrowOnInvalidChar(_target.TargetString[i]);

                    // Check if matches pattern
                    // todo: Use more optimal algorithm to find patterns (this thing I got in IT lessons)
                    bool patternMatches = false;
                    for (int j = 0; j < _pattern.Length; i++)
                    {
                        if (_target.TargetString[i + j] != _pattern[j])
                            break;

                        patternMatches = true;
                    }

                    if (patternMatches)
                    {
                        _current = new StringSegment(_target.TargetString, _position, i - _position);
                        _position += i + _pattern.Length;

                        return true;
                    }
                }
                
                // End of string
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
                PatternSplittersPool.Push(this);
            }

            private void ThrowOnInvalidChar(char c)
            {
                throw new InvalidCharacterException(c);
            }
        }
    }
}