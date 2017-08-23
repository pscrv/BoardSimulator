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
        private BoardParameters _boardParameters;
        private BasicSetupViewModel _basicSetupVM;

        public ViewModel CurrentVM { get; private set; }
        #endregion

        #region commands
        public ICommand FullSimulationCommand { get => new DelegateParamterisedCommand(_fullSim, _fullSimActive); }

        public ICommand SetupCommand { get => new DelegateParamterisedCommand(_setup, _setupActive); }

        private void _fullSim(object parameter)
        {
            CurrentVM = new FullSimulationViewModel(_boardParameters);
            OnPropertyChanged("CurrentVM");
        }

        private void _setup(object parameter)
        {
            _basicSetupVM.Reset();
            CurrentVM = _basicSetupVM;
            OnPropertyChanged("CurrentVM");
        }

        private bool _setupActive(object obj)
        {
            return CurrentVM != _basicSetupVM;
        }

        private bool _fullSimActive(object obj)
        {
            return CurrentVM == _basicSetupVM;
        }
        #endregion

        public MainViewModel()
        {
            _boardParameters = BoardParameters.__DefaultParameters();
            _basicSetupVM = new BasicSetupViewModel(_boardParameters);
            CurrentVM = _basicSetupVM;
        }
    }
}
