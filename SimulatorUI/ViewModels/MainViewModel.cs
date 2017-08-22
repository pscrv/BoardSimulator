using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SimulatorUI
{

    public class MainViewModel : ViewModel
    {
        #region fields and properties
        private BasicSetupViewModel _basicSetupVM;

        public ViewModel CurrentVM { get; private set; }
        #endregion

        #region commands
        public ICommand FullSimulationCommand { get => new DelegateParamterisedCommand(_fullSim); }
        private ICommand _switchToFullSim { get => new DelegateParamterisedCommand(_fullSim); }
        private void _fullSim(object parameter)
        {
            CurrentVM = new FullSimulationViewModel(_basicSetupVM.BoardParametersVM);
            OnPropertyChanged("CurrentVM");
        }


        public ICommand SetupCommand { get => new DelegateParamterisedCommand(_setup); }
        private ICommand _switchtoBasicSetup { get => new DelegateParamterisedCommand(_setup); }
        private void _setup(object parameter)
        {
            CurrentVM = new BasicSetupViewModel();
            OnPropertyChanged("CurrentVM");
        }
        #endregion

        public MainViewModel()
        {
            _basicSetupVM = new BasicSetupViewModel();
            CurrentVM = _basicSetupVM;
        }
    }
}
