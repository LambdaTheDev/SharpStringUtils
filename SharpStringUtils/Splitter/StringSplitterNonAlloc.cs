using System;
using System.Collections;
using System.Collections.Generic;
using LambdaTheDev.SharpStringUtils.Segment;

namespace LambdaTheDev.SharpStringUtils.Splitter
{
    public readonly partial struct StringSplitterNonAlloc : IEnumerable<StringSegment>
    {
        private readonly Func<char, bool> _charsValidator;
        private readonly StringSegment _segment;
        private readonly char _separator;
        private readonly string _pattern;


        public StringSplitterNonAlloc(StringSegment segment, char separator, Func<char, bool> charsValidator)
        {
            if (charsValidator == null)
                charsValidator = AnyCharPasses;

            _charsValidator = charsValidator;
            _segment = segment;
            _pattern = null;
            _separator = separator;
        }

        public StringSplitterNonAlloc(StringSegment segment, string pattern, Func<char, bool> charsValidator)
        {
            if (charsValidator == null)
                charsValidator = AnyCharPasses;

            _charsValidator = charsValidator;
            _segment = segment;
            _pattern = pattern;
            _separator = default;
        }

        public IEnumerator<StringSegment> GetEnumerator()
        {
            ISplitter result;
            
            if (_pattern == null)
            {
                if (!SeparatorSplittersPool.TryPop(out SeparatorSplitter splitter))
                    splitter = new SeparatorSplitter();

                splitter.Separator = _separator;
                result = splitter;
            }
            else
            {
                if (!PatternSplittersPool.TryPop(out PatternSplitter splitter))
                    splitter = new PatternSplitter();
                
                result = splitter;
            }
            
            result.Prepare(_segment, _charsValidator);
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static bool AnyCharPasses(char _) => true;
        
        
        private interface ISplitter : IEnumerator<StringSegment>
        {
            void Prepare(StringSegment segment, Func<char, bool> validator);
        }
    }
}