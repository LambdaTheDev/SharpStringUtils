using System.Collections;
using System.Collections.Generic;

/*
 * ORIGINALLY CODED BY: ConnorTron110 (https://github.com/ConnorTron110)
 * Licensed under MIT license.
 * Minor optimization & naming changes made by LambdaTheDev
 */

namespace LambdaTheDev.SharpStringUtils.Iterator
{
    // Minimal or zero alloc string iterator
    public struct StringSplitterNonAlloc : IEnumerable<StringSegment>
    {
        private readonly string _target;
        private readonly char _separator;

        private StringSegment _currentEntry;
        private int _position;
        private bool _ended;


        public StringSplitterNonAlloc(string target, char separator)
        {
            _target = target;
            _separator = separator;

            _currentEntry = StringSegment.Null;
            _position = 0;
            _ended = false;
        }

        public StringSegment Current()
        {
            return _currentEntry;
        }

        public bool MoveNext()
        {
            // Ending condition
            if (_ended)
                return false;

            // Validation
            if (_target == null)
            {
                _currentEntry = StringSegment.Null;
                _ended = true;
                return true;
            }

            if (_target == string.Empty)
            {
                _currentEntry = StringSegment.Empty;
                _ended = true;
                return true;
            }

            // Actual iteration through string
            for (int i = _position; i < _target.Length; i++)
            {
                if (_target[i] == _separator)
                {
                    _currentEntry = new StringSegment(_target, _position, i - _position);
                    _position = i + 1;

                    return true;
                }
            }

            // End of string -> wrap everything that it left
            _currentEntry = new StringSegment(_target, _position, _target.Length - _position);
            _ended = true;

            return true;
        }

        public IEnumerator<StringSegment> GetEnumerator()
        {
            while (MoveNext())
            {
                yield return Current();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}