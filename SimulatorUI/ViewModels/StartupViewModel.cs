using System.Threading.Tasks;

using Simulator;

namespace SimulatorUI
{
    internal class StartupViewModel : ViewModel
    {
        #region static
        private static int __miniSimulationLengthInYears = 1;
        private static int __initialCaseCount = 100;
        private static int __monthlyArrivals = 10;
        #endregion




        #region fields and properties
        private Simulation _miniSim;
        
        private BoardParametersViewModel _boardParametersVM;
        private SimulationParametersViewModel _miniSimParametersVM;
        private SimulationReportViewModel _reportVM;

        private DebouncedHandler _debouncedHandler = new DebouncedHandler();


        public BoardParametersViewModel BoardParametersVM { get => _boardParametersVM; }  

        public SimulationParametersViewModel SimulationParametersVM { get => _miniSimParametersVM; }

        public SimulationReportViewModel SimulationReportVM { get => _reportVM; }
        #endregion





        #region construction
        public StartupViewModel()
        {
            _boardParametersVM = BoardParametersViewModel.MakeDefaultBoard();

            _miniSimParametersVM = new SimulationParametersViewModel
            {
                InitialCaseCount = __initialCaseCount,
                ArrivalsPerMonth = __monthlyArrivals,
                MiniRunLength = __miniSimulationLengthInYears,
            };

            _boardParametersVM.PropertyChanged += (s, e) => _runMiniSim();
            _boardParametersVM.PropertyChanged += (s, e) => _raisePropertyChanged();

            _miniSimParametersVM.PropertyChanged += (s, e) => _runMiniSim();
            _miniSimParametersVM.PropertyChanged += (s, e) => _raisePropertyChanged();

            _runMiniSim();
        }
        #endregion


        private void _runMiniSim()
        {
            Task task = new Task(
                () => _runSim(_miniSimParametersVM.MiniRunLength));

            _debouncedHandler.Handle(task);
        }

        private void _runSim(int length)
        {
            var x = _boardParametersVM.DetailsVM.Parameters;
            var y = x.AsSimulatorBoardParameters;

            _miniSim = Simulation.MakeSimulation(
                length,
                _boardParametersVM.DetailsVM.Parameters.AsSimulatorBoardParameters,  //TODO: improve this
                _miniSimParametersVM.InitialCaseCount,
                _miniSimParametersVM.ArrivalsPerMonth);
            _miniSim.Run();
            _reportVM = new SimulationReportViewModel(_miniSim.SimulationReport);
            _raisePropertyChanged();
        }


        private void _raisePropertyChanged()
        {
            OnPropertyChanged("SimulationReportVM");
        }
    }
}