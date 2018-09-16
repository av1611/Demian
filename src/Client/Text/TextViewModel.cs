using System.IO;
using System.Windows.Documents;
using Pocket.Wpf;

namespace Demian.Client
{
    public class TextViewModel : ViewModel
    {
        private readonly FlowDocument _document;

        public TextViewModel(FlowDocument document)
        {
            Text = new InMemoryText("");
            
            _document = document;
        }

        public IText Text { get; private set; }

        public void Print()
        {
            var content = new Run(Text.AsString());
            var paragraph = new Paragraph(content);
            
            _document.Blocks.Clear();
            _document.Blocks.Add(paragraph);
        }

        public void Save(string path)
        {
            File.WriteAllText(path, Text.AsString());
        }

        public void Load(string path)
        {
            var text = File.ReadAllText(path);

            Text = new InMemoryText(text);
            
            Print();
        }
    }
}