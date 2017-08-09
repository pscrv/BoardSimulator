using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimulatorUI
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(
                    this, 
                    new PropertyChangedEventArgs(propertyName));
            }
        }


        protected void SetProperty<T>(ref T backingfield, T value, string propertyName)
        {
            if (Equals(backingfield, value))
                return;

            backingfield = value;
            OnPropertyChanged(propertyName);            
        }
        
    }





    //public class DelegateActionCommand : ICommand
    //{

    //    private readonly Action _action;
    //    public event EventHandler CanExecuteChanged;

    //    public DelegateActionCommand(Action action)
    //    {
    //        _action = action;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        _action();
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    //        return true;
    //    }        
    //}



    public class DelegateParamterisedCommand : ICommand
    {
        public static int __InstanceCount = 0;
        public static int __CanExecuteCount = 0;

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
            __InstanceCount++;
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
            __CanExecuteCount++;
            var x = __CanExecuteCount;
            return _canExecute(parameter);
        }
    }
}
