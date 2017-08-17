using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimulatorUI
{   
    public class DelegateParamterisedCommand : ICommand
    {
        public delegate bool Enabler(object obj);
        public delegate void Executable(object parameter);
        private readonly Enabler _canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Executable _execute;

        public DelegateParamterisedCommand(Executable execute, Enabler canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateParamterisedCommand(Executable execute) 
            : this(execute, p => true)
        { }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }
    }
}
