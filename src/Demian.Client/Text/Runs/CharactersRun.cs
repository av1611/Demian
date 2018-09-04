using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Demian.Client
{
    public class CharactersRun : Run
    {
        public CharactersRun(IEnumerable<Character> characters)
            : base(new string(characters.Select(x => x.Value).ToArray())) { }
    }
}