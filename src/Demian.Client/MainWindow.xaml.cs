using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

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
            var dialog = new SaveFileDialog
            {
                FileName = "TextDocument",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            var saved = dialog.ShowDialog();
            if (saved.GetValueOrDefault())
                _viewModel.Save(dialog.FileName);
        }

        private void OnLoad(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            var opened = dialog.ShowDialog();
            if (opened.GetValueOrDefault())
                _viewModel.Load(dialog.FileName);
        }
    }
}