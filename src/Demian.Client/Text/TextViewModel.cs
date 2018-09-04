using System.IO;
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
            var content = new Run(_text.AsString());
            var paragraph = new Paragraph(content);
            
            _document.Blocks.Clear();
            _document.Blocks.Add(paragraph);
        }

        public void Save(string path)
        {
            File.WriteAllText(path, _text.AsString());
        }
    }
}