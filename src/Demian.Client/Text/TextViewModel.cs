using System.Windows.Documents;
using Pocket.Wpf;

namespace Demian.Client
{
    public class TextViewModel : ViewModel
    {
        private readonly IText _text;
        private readonly FlowDocument _document;
        
        public TextViewModel(FlowDocument document)
        {
            _text = new ConstText("Hello!");
            _document = document;
        }

        public void Print()
        {
            var content = new CharactersRun(_text.Characters);
            var paragraph = new Paragraph(content);
            
            _document.Blocks.Clear();
            _document.Blocks.Add(paragraph);
        }
    }
}