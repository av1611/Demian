using System.Windows;
using System.Windows.Input;

namespace Demian.Client
{
    public partial class MainWindow
    {
        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand LoadCommand = new RoutedCommand();
        
        private TextEditor _editor;
        private TextViewModel _viewModel;

        static MainWindow()
        {
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            LoadCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
        }
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnTextEditorLoad(object sender, RoutedEventArgs e)
        {
            _editor = (TextEditor) sender;
            
            _viewModel = new TextViewModel(_editor.Document);
            _viewModel.Print();
        }

        private void OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save");
        }

        private void OnLoad(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Load");
        }
    }
}