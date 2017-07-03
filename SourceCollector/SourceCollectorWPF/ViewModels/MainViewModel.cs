using Prism.Commands;
using CodeHighlighter.Adapters;
using CodeHighlighter.BusinessLogic;
using CodeHighlighter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeHighlighter.ViewModels
{
    class MainViewModel : IProgress<double>
    {
        public MainModel Model { get; set; }
        public AutoCanExecuteCommandAdapter StartCommand { get; set; }
        public DelegateCommand BrowseSourceDirCommand { get; set; }
        public DelegateCommand BrowseOutputFileCommand { get; set; }

        public MainViewModel()
        {
            Model = new MainModel()
            {
                SkipPattern = "bootstrap|jquery",
                IsWorking = false
            };

            StartCommand = new AutoCanExecuteCommandAdapter(
                new DelegateCommand(async () => await DoStart(), () =>
                    !Model.IsWorking &&
                    !string.IsNullOrEmpty(Model.SourceDirectory) &&
                    !string.IsNullOrEmpty(Model.OutputFile)
                )
            );

            BrowseSourceDirCommand = new DelegateCommand(DoBrowseSourceDir);
            BrowseOutputFileCommand = new DelegateCommand(DoBrowseOutputFile);
        }

        private void DoBrowseOutputFile()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Html files|*.html;*.xhtml;*.htm"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Model.OutputFile = dialog.FileName;
            }
        }
        private void DoBrowseSourceDir()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Model.SourceDirectory = dialog.SelectedPath;
                }
            }
        }

        private async Task DoStart()
        {
            Model.IsWorking = true;
            Model.Progess = 0.0;
            Model.Status = "";

            (var totalFiles, var handledFiles) = await new FileHandler().CreateHighlightedHtmlOfContentsAsync(
                Model.SourceDirectory, Model.SearchPattern,
                Model.SkipPattern, Model.OutputFile, this);

            if (handledFiles < totalFiles)
            {
                Model.Status = $"Finsihed with an error. [{handledFiles}/{totalFiles}]";
            }
            else if (totalFiles < 0)
            {
                Model.Status = $"Any input Parameter is not correct!";
            }
            else
            {
                Model.Status = $"Finsihed! [{handledFiles}/{totalFiles}]";
            }

            Model.IsWorking = false;
        }

        public void Report(double value)
        {
            Model.Progess = value;
        }
    }
}
