using Prism.Mvvm;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CodeHighlighter.Models
{
    internal class MainModel : BindableBase
    {
        private string sourceDirectory;
        public string SourceDirectory {
            get { return sourceDirectory; }
            set { SetProperty(ref sourceDirectory, value); }
        }

        private string searchPattern;
        public string SearchPattern {
            get { return searchPattern; }
            set { SetProperty(ref searchPattern, value); }
        }

        private string skipPattern;
        public string SkipPattern {
            get { return skipPattern; }
            set { SetProperty(ref skipPattern, value); }
        }


        private string outputFile;
        public string OutputFile {
            get { return outputFile; }
            set { SetProperty(ref outputFile, value); }
        }

        private double progess;
        public double Progess {
            get { return progess; }
            set { SetProperty(ref progess, value); }
        }

        private string status;
        public string Status {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private bool isWorking;
        public bool IsWorking {
            get { return isWorking; }
            set { SetProperty(ref isWorking, value); }
        }
    }
}