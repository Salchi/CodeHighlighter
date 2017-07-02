using SourceCollectorWPF.BusinessLogic;
using SourceCollectorWPF.Models;
using System;
using System.Windows;
using System.Windows.Forms;

namespace SourceCollectorWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model model;
        public MainWindow()
        {
            InitializeComponent();
            model = new Model()
            {
                SkipPattern = "bootstrap|jquery"
            };
            DataContext = model;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var numberOfFiles = await new FileCollector().CopyContents(model.SourceDirectory, model.SearchPattern, model.SkipPattern, model.OutputFile);
                model.Status = $"Copied content of {numberOfFiles} files to '{model.OutputFile}'";
            }
            catch(Exception ex)
            {
                model.Status = ex.Message;
            }
        }

        private void SourceDirBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    model.SourceDirectory = dialog.SelectedPath;
                }
            }
        }
        private void OutputFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                model.OutputFile = dialog.FileName;
            }
        }
    }
}
