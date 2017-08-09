using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Simulator;

namespace SimulatorUI
{
    public class SimulationViewModel : ObservableObject
    {
        #region static
        private static int __miniSimulationLength = 8 * 22 * 12;
        private static int __initialCaseCount = 100;    
        #endregion



        #region fields and properties
        private Simulation _simulation;
        private BoardParametersViewModel _boardVM;
        private int _initialCaseCount;


        public BoardParametersViewModel BoardVM
        {
            get => _boardVM; 
            //private set => SetProperty(ref _boardVM, value, "BoardVM");
        }
        


        public int InitialCaseCount
        {
            get => _initialCaseCount;
            set
            {
                SetProperty(ref _initialCaseCount, value, "InitialCaseCount");
                _miniSim();
                _raisePropertyChanged();
            }
        }


        public int FinishedCaseCount
        {
            get { return _simulation.FinishedCases.Count; }
        }
        #endregion


        

        #region construction
        public SimulationViewModel()
        {
            _boardVM = BoardParametersViewModel.MakeDefaultBoard();
            _initialCaseCount = __initialCaseCount;
            _miniSim();

            _boardVM.PropertyChanged += (s, e) => _miniSim();
            _boardVM.PropertyChanged += (s, e) => _raisePropertyChanged();
            //this.PropertyChanged += (s, e) => _miniSim();
            //this.PropertyChanged += (s, e) => _raisePropertyChanged();

        }
        #endregion
        


        private void _miniSim()
        {
            _simulation = new Simulation(__miniSimulationLength, _boardVM.Parameters.AsSimulatorBoardParameters, _initialCaseCount);
            _simulation.Run();
        }


        private void _raisePropertyChanged()
        { 
            OnPropertyChanged("FinishedCaseCount");
        }
    }
}