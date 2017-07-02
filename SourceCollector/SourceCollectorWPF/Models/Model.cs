using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SourceCollectorWPF.Models
{
    internal class Model : INotifyPropertyChanged
    {
        private string sourceDirectory;
        public string SourceDirectory {
            get { return sourceDirectory; }
            set {
                sourceDirectory = value;
                OnPropertyChanged();
            }
        }

        private string searchPattern;
        public string SearchPattern {
            get { return searchPattern; }
            set {
                searchPattern = value;
                OnPropertyChanged();
            }
        }

        private string skipPattern;
        public string SkipPattern {
            get { return skipPattern; }
            set {
                skipPattern = value;
                OnPropertyChanged();
            }
        }


        private string outputFile;
        public string OutputFile {
            get { return outputFile; }
            set {
                outputFile = value;
                OnPropertyChanged();
            }
        }

        private string status;
        public string Status {
            get { return status; }
            set {
                status = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}