using System.Collections.Generic;
using System.Linq;

namespace Demian
{
    public sealed class ConstText : IText
    {
        public ConstText(string text)
        {
            Characters = text
                .Select(x => new Character(x))
                .ToList();
        }
        
        public IEnumerable<Character> Characters { get; }
    }
}