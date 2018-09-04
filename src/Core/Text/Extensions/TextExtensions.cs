using System.Linq;
using Pocket.Common;

namespace Demian
{
    public static class TextExtensions
    {
        public static string AsString(this IText self) =>
            self.Characters
                .Select(x => x.Value)
                .ToArray()
                .As(x => new string(x));
    }
}