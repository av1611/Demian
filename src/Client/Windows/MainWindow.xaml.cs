using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Demian.Client.Common;
using Microsoft.Win32;
using Pocket.Common;

namespace Demian.Client
{
    public partial class MainWindow
    {
        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand LoadCommand = new RoutedCommand();
        public static readonly RoutedCommand ConsoleOpenCommand = new RoutedCommand();

        private readonly ILog _log;
        private readonly ConsoleWindow _console;
        
        private TextEditor _editor;
        private TextViewModel _viewModel;

        private bool _loaded;

        static MainWindow()
        {
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            LoadCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            ConsoleOpenCommand.InputGestures.Add(new KeyGesture(Key.OemTilde, ModifierKeys.Control));
        }
        
        public MainWindow()
        {
            // TODO: Initialize console only in Debug mode.
            _console = new ConsoleWindow();
            _console.Hide();
            
            _log = new ConsoleWindowLog(_console);
            
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
                _log.Info($"Trying to apply [ Offset: {change.Offset} | Added: {change.AddedLength} | Removed: {change.RemovedLength} ] text change.");
                
                var maybeRange = change.AsRangeIn(_editor.Document);
                if (maybeRange.IsNothing)
                    continue;

                // TODO: Fix offset calculation for more paragraphs and lines.
                var paragraph = maybeRange.Value.Start.Paragraph;
                var offset = paragraph?.ContentStart.GetOffsetToPosition(maybeRange.Value.Start) - 1 ?? 0;

                if (change.AddedLength > 0)
                {
                    _log.Info($"Adding \"{maybeRange.Value.Text}\" at [ {offset} ] offset.");
                    
                    var written = _viewModel.Text.Write(maybeRange.Value.Text, offset);
                    if (written.Fail)
                        _log.Info($"Failed to write: {written.Error}.");
                }
                else
                {
                    var length = change.RemovedLength.Or(_viewModel.Text.Characters.Count()).IfGreater();
                    
                    _log.Info($"Removing [ {length} ] characters at [ {offset} ] offset.");

                    var removed = _viewModel.Text.Remove(length, offset);
                    if (removed.Fail)
                        _log.Info($"Failed to remove: {removed.Error}.");
                }
            }
        }

        private void OnConsoleOpen(object sender, ExecutedRoutedEventArgs e)
        {
            if (_console.IsVisible)
                _console.Hide();
            else
                _console.Show();
        }
    }
}