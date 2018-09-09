using System.Collections.Generic;
using Pocket.Common;

namespace Demian
{
    public interface IText
    {
        IEnumerable<Character> Characters { get; }

        Result Write(string value, int at);
    }
}