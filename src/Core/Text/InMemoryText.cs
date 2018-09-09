using System.Collections.Generic;
using System.Linq;
using Pocket.Common;

namespace Demian
{
    public sealed class InMemoryText : IText
    {
        private struct Change
        {
            public static Change Add(string value, int at) => new Change(value, at, 0);
            public static Change Remove(int at, int count) => new Change(null, at, count);
            
            private readonly string _value;
            private readonly int _index;
            private readonly int _count;

            public Change(string value, int index, int count)
            {
                _value = value;
                _index = index;
                _count = count;
            }

            public void Apply(InMemoryText text)
            {
                // Add.
                if (_value != null)
                    text._characters.InsertRange(_index, _value.Select(x => new Character(x)));
                else
                    text._characters.RemoveRange(_index, _count);
            }
        }
        
        private readonly List<Character> _characters;
        private readonly List<Change> _changes;
        
        private int _length;
        
        public InMemoryText(string text)
        {
            _characters = text
                .Select(x => new Character(x))
                .ToList();
            _length = _characters.Count;
            _changes = new List<Change>();
        }

        public IEnumerable<Character> Characters
        {
            get
            {
                _changes.ForEach(x => x.Apply(this));
                _changes.Clear();
                _length = _characters.Count;

                return _characters;
            }
        }
        
        public Result Write(string value, int at)
        {
            if (value == null)
                return Result.Failed("Couldn't insert null.");
            if (at < 0)
                return Result.Failed($"Couldn't insert \"{value}\": index [ {at} ] is less than zero.");
            if (at > _length)
                return Result.Failed($"Couldn't insert \"{value}\": index [ {at} ] is greater than [ {_length} ] length.");
            
            _changes.Add(Change.Add(value, at));
            _length += value.Length;
            
            return Result.Succeded();
        }
    }
}