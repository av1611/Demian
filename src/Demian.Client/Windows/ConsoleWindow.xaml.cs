using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Demian.Client
{
    public partial class ConsoleWindow
    {
        public static readonly RoutedCommand ConsoleOpenCommand = new RoutedCommand();
        
        private TextBlock _text;
        
        static ConsoleWindow()
        {
            ConsoleOpenCommand.InputGestures.Add(new KeyGesture(Key.OemTilde, ModifierKeys.Control));
        }
        
        public ConsoleWindow()
        {
            InitializeComponent();
        }
        
        private void OnTextBlockLoad(object sender, RoutedEventArgs e)
        {
            _text = (TextBlock) e.Source;
        }

        public void Add(Inline inline)
        {
            _text.Inlines.Add(inline);
        }
        
        private void OnConsoleOpen(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsVisible)
                Hide();
            else
                Show();
        }
    }
}
