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
    public partial class MainWindow : Window, IProgress<double>
    {
        private Model model;
        public MainWindow()
        {
            InitializeComponent();
            model = new Model()
            {
                SkipPattern = "bootstrap|jquery",
                IsStartButtonEnabled = true
            };
            DataContext = model;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            model.IsStartButtonEnabled = false;
            model.Progess = 0.0;
            model.Status = "";

            try
            {
                await new FileHandler().CreateHighlightedHtmlOfContentsAsync(model.SourceDirectory, model.SearchPattern,
                    model.SkipPattern, model.OutputFile, this);

                model.Status = "Finsihed!";
            }
            catch (Exception)
            {
                model.Status = "Finsihed with an error.";
            }
            model.IsStartButtonEnabled = true;
        }

        public void Report(double value)
        {
            model.Progess = value;
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
