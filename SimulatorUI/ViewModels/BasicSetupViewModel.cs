using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Simulator;

namespace SimulatorUI
{
    public class BasicSetupViewModel : ViewModel
    {
        #region static
        private static int __miniSimulationLengthInYears = 1;
        private static int __initialCaseCount = 100;
        private static int __monthlyArrivals = 10;
        #endregion


        #region fields and properties
        private BoardParameters _boardParameters;
        private Simulation _miniSim;
        private bool _miniSimIsRunning;
        
        public BoardParametersViewModel BoardParametersVM { get; private set; }  
        public SimulationParametersViewModel SimulationParametersVM { get; private set; }
        public SimulationReportViewModel SimulationReportVM { get; private set; }    
        
        public bool MiniSimIsRunning
        {
            get => _miniSimIsRunning;
            set { SetProperty(ref _miniSimIsRunning, value, "MiniSimIsRunning"); }
        }

        private DebouncedHandler _debouncedHandler = new DebouncedHandler();
        #endregion



        #region construction

        public BasicSetupViewModel(BoardParameters parameters)
        {
            _boardParameters = parameters;
            BoardParametersVM = new BoardParametersViewModel(parameters);

            SimulationParametersVM = new SimulationParametersViewModel
            {
                InitialCaseCount = __initialCaseCount,
                ArrivalsPerMonth = __monthlyArrivals,
                MiniRunLength = __miniSimulationLengthInYears
            };

            BoardParametersVM.PropertyChanged += (s, e) => _runMiniSim();
            BoardParametersVM.PropertyChanged += (s, e) => _raisePropertyChanged();

            SimulationParametersVM.PropertyChanged += (s, e) => _runMiniSim();
            SimulationParametersVM.PropertyChanged += (s, e) => _raisePropertyChanged();

            MiniSimIsRunning = true;
            _runMiniSim();
            MiniSimIsRunning = false;
        }
        #endregion


        public void Reset()
        {
            BoardParametersVM.Reset();
        }
        


        private void _runMiniSim()
        {
            Task task = new Task(
                () => _runSim(SimulationParametersVM.MiniRunLength));

            _debouncedHandler.Handle(task);
        }

        private void _runSim(int length)
        {
            MiniSimIsRunning = true;
            _miniSim = Simulation.MakeSimulation(
                length,
                BoardParametersVM.DetailsVM.Parameters.AsSimulatorBoardParameters,  
                SimulationParametersVM.InitialCaseCount,
                SimulationParametersVM.ArrivalsPerMonth);
            _miniSim.Run();
            SimulationReportVM = new SimulationReportViewModel(_miniSim.SimulationReport);
            MiniSimIsRunning = false;
            _raisePropertyChanged();
        }


        private void _raisePropertyChanged()
        {
            OnPropertyChanged("SimulationReportVM");
        }
    }
}