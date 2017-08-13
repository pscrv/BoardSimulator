using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Simulator;

namespace SimulatorUI
{
    public class SimulationViewModel : ObservableObject
    {
        #region static
        private static int __miniSimulationLengthInYears = 1;
        private static int __fullSimultionLengthInYears = 10;
        private static int __initialCaseCount = 100;
        private static int __monthlyArrivals = 10;
        #endregion



        #region fields and properties
        private Simulation _simulation;
        private BoardParametersViewModel _boardVM;
        private SimulationParametersViewModel _simulationParametersVM;
        private SimulationReportViewModel _miniSimReportVM;
        
        private object _lock = new object();
        private DebouncedHandler _debouncedHandler = new DebouncedHandler();


        public BoardParametersViewModel BoardVM { get => _boardVM; }

        public SimulationParametersViewModel SimulationParametersVM { get => _simulationParametersVM; }

        public SimulationReportViewModel SimulationReportVM { get => _miniSimReportVM; }
        
        
        public int FinishedCaseCount
        {
            get
            {
                return _miniSimReportVM?.FinishedCaseCount ?? 0;
                //return _simulation?.FinishedCases.Count ?? 0;
            }
        }
        #endregion



        #region Command
        public ICommand FullSimulationCommand
        {
            get => new DelegateParamterisedCommand(x => _fullSim());
        }
        #endregion



        #region construction
        public SimulationViewModel()
        {
            _boardVM = BoardParametersViewModel.MakeDefaultBoard();
            _simulationParametersVM = new SimulationParametersViewModel
            {
                InitialCaseCount = __initialCaseCount,
                ArrivalsPerMonth = __monthlyArrivals,
                MiniRunLength = __miniSimulationLengthInYears,
                FullRunLength = __fullSimultionLengthInYears
            };

            
            _boardVM.PropertyChanged += (s, e) => _miniSim();
            _boardVM.PropertyChanged += (s, e) => _raisePropertyChanged();

            _simulationParametersVM.PropertyChanged += (s, e) => _miniSim();
            _simulationParametersVM.PropertyChanged += (s, e) => _raisePropertyChanged();
            
            _miniSim();
        }
        #endregion
        
        private void _miniSim()
        {
            Task task = new Task(
                () => _runSim(_simulationParametersVM.MiniRunLength));

            _debouncedHandler.Handle(task);
        }


        private void _fullSim()
        {
            Task.Run(
                () => _runSim(_simulationParametersVM.FullRunLength));
        }


        private void _runSim(int length)
        {
            _simulation = Simulation.MakeSimulation(
                length,
                _boardVM.Parameters.AsSimulatorBoardParameters,
                _simulationParametersVM.InitialCaseCount,
                _simulationParametersVM.ArrivalsPerMonth);
            _simulation.Run();
            _miniSimReportVM = new SimulationReportViewModel(_simulation.SimulationReport);
            _raisePropertyChanged();
        }

        

        private void _raisePropertyChanged()
        { 
            OnPropertyChanged("FinishedCaseCount");
        }
    }
}