using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SourceCollectorWPF.Adapters
{
    class AutoCanExecuteCommandAdapter : ICommand
    {
        public ICommand Adaptee { get; private set; }

        public AutoCanExecuteCommandAdapter(ICommand adaptee)
        {
            Adaptee = adaptee ?? throw new ArgumentNullException(nameof(adaptee));
        }

        public void Execute(object parameter)
        {
            Adaptee.Execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return Adaptee.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
