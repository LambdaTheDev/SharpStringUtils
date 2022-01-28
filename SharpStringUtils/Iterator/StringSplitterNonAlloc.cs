using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/*
 * ORIGINALLY CODED BY: ConnorTron110 (https://github.com/ConnorTron110)
 * Licensed under MIT license.
 * Minor optimization & naming changes made by LambdaTheDev
 */

namespace LambdaTheDev.SharpStringUtils.Iterator
{
    // Minimal or zero alloc string iterator
    // todo: Make SplitFirst() method & make different struct for enumerator
    public struct StringSplitterNonAlloc : IEnumerator<StringSegment>, IEnumerable<StringSegment>
    {
        private readonly string _target;
        private readonly char _separator;
        private readonly Func<char, bool> _charValidator;

        private StringSegment _currentEntry;
        private int _position;
        private bool _ended;


        public StringSplitterNonAlloc(string target, char separator, Func<char, bool> charValidator = null)
        {
            _target = target;
            _separator = separator;

            _currentEntry = StringSegment.Null;
            _position = 0;
            _ended = false;

            if (charValidator == null)
                charValidator = AnyCharPasses;

            _charValidator = charValidator;
        }

        #region String iterator

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                if(!_charValidator.Invoke(_target[i]))
                    ThrowValidationFailed(_target[i]);
                
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

        #endregion

        #region Rest of IEnumerator/Enumerable implementations

        public void Reset()
        {
            _currentEntry = StringSegment.Null;
            _ended = false;
            _position = 0;
        }

        StringSegment IEnumerator<StringSegment>.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _currentEntry;
        }


        object IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _currentEntry;
        }

        public IEnumerator<StringSegment> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

        public void Dispose() { }

        #endregion

        
        // Method that marks every character as valid
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AnyCharPasses(char _) => true;

        // Throws exception when string contains character marked as invalid by validator method
        private static void ThrowValidationFailed(char c)
        {
            throw new ArgumentException("Provided string contains invalid char: " + c + "!");
        }
    }
}