using System.Windows.Controls;
using System.Windows.Documents;
using Pocket.Common;

namespace Demian.Client
{
    public static class TextChangeExtensions
    {
        public static Maybe<TextRange> AsRangeIn(this TextChange self, FlowDocument document)
        {
            if (self.AddedLength == self.RemovedLength)
                return Maybe<TextRange>.Nothing;

            var start = document.ContentStart.GetPositionAtOffset(self.Offset);
            var endOffset = self.AddedLength > 0
                ? self.Offset + self.AddedLength
                : self.Offset + self.RemovedLength;
            var end = document.ContentStart.GetPositionAtOffset(endOffset);
            if (end == null)
                return Maybe<TextRange>.Nothing;

            return new TextRange(start, end).Maybe();
        }
    }
}