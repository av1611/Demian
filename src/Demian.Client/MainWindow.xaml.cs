using System.Windows;

namespace Demian.Client
{
    public partial class MainWindow
    {
        private TextEditor _editor;
        private TextViewModel _viewModel;
        
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
    }
}