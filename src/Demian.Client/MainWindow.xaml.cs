using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

        private bool _loaded;

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

            _loaded = true;
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

        private void OnTextEditorChange(object sender, TextChangedEventArgs e)
        {
            if (!_loaded)
                return;
            
            foreach (var change in e.Changes)
            {
                var maybeRange = change.AsRangeIn(_editor.Document);
                if (maybeRange.IsNothing)
                    continue;

                var paragraph = maybeRange.Value.Start.Paragraph;
                var offset = paragraph.ContentStart.GetOffsetToPosition(maybeRange.Value.Start) - 1;

                if (change.AddedLength > 0)
                    _viewModel.Text.Write(maybeRange.Value.Text, offset);
            }
        }
    }
}