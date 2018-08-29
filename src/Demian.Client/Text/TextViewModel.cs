using System.Windows.Documents;
using Pocket.Wpf;

namespace Demian.Client
{
    public class TextViewModel : ViewModel
    {
        private readonly IText _text;
        
        public TextViewModel()
        {
            _text = new ConstText("Hello!");
        }

        public void Print(FlowDocument document)
        {
            var content = new Run(_text.Content);
            var paragraph = new Paragraph(content);
            
            document.Blocks.Clear();
            document.Blocks.Add(paragraph);
        }
    }
}