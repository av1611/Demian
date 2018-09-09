using System.Collections.Generic;
using System.Linq;
using Pocket.Common;

namespace Demian
{
    public sealed class InMemoryText : IText
    {
        private struct Change
        {
            public static Change Add(string value, int at) => new Change(value, 0, at);
            public static Change Remove(int count, int at) => new Change(null, count, at);
            
            private readonly string _value;
            private readonly int _index;
            private readonly int _count;

            public Change(string value, int count, int index)
            {
                _value = value;
                _count = count;
                _index = index;
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

        public Result Remove(int count, int at)
        {
            if (count <= 0)
                return Result.Failed($"Couldn't remove characters at [ {at} ]: specified [ {count} ] length is less or equals to zero.");
            if (at < 0)
                return Result.Failed($"Couldn't remove [ {count} ] characters: index [ {at} ] is less than zero.");
            if (at + count > _length)
                return Result.Failed($"Couldn't remove [ {count} ] characters at [ {at} ]: there are only [ {_length} ] of them.");
            
            _changes.Add(Change.Remove(count, at));
            _length -= count;
            
            return Result.Succeded();
        }
    }
}