using Prism.Commands;
using SourceCollectorWPF.BusinessLogic;
using SourceCollectorWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceCollectorWPF.ViewModels
{
    class MainViewModel : IProgress<double>
    {
        public MainModel Model { get; set; }
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand BrowseSourceDirCommand { get; set; }
        public DelegateCommand BrowseOutputFileCommand { get; set; }

        public MainViewModel()
        {
            Model = new MainModel()
            {
                SkipPattern = "bootstrap|jquery",
                IsStartButtonEnabled = true
            };

            StartCommand = new DelegateCommand(async () => await DoStart(), () => Model.IsStartButtonEnabled);
            BrowseSourceDirCommand = new DelegateCommand(DoBrowseSourceDir);
            BrowseOutputFileCommand = new DelegateCommand(DoBrowseOutputFile);
        }

        private void DoBrowseOutputFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
            Model.IsStartButtonEnabled = false;
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

            Model.IsStartButtonEnabled = true;
        }

        public void Report(double value)
        {
            Model.Progess = value;
        }
    }
}
