using System.Collections.Generic;

namespace Demian
{
    public interface IText
    {
        IEnumerable<Character> Characters { get; }
    }
}