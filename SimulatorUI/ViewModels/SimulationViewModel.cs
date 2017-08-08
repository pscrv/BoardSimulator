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

        private static ChairType __type = ChairType.Legal;
        private static MemberParameterCollection __chair = new MemberParameterCollection(
            new MemberParameters(6, 6, 12),
            new MemberParameters(40, 8, 24),
            new MemberParameters(3, 4, 8));


        private static MemberParameterCollection __defaultTechnical()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }

        private static MemberParameterCollection __defaultLegal()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }


        private static List<MemberParameterCollection> __technicals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10))
            };

        private static List<MemberParameterCollection> __legals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10)),

                new MemberParameterCollection(
                    new MemberParameters(9, 9, 15),
                    new MemberParameters(43, 11, 27),
                    new MemberParameters(6, 7, 11))
            };

        #endregion



        #region fields and properties
        private Simulation _simulation;
        private BoardParametersViewModel _boardVM;
        
        

        public BoardParametersViewModel BoardVM
        {
            get { return _boardVM; }
            private set { SetProperty(ref _boardVM, value, "BoardVM"); }
        }

        public ChairType ChairType
        {
            get { return _boardVM.ChairType; }
        }


        public MemberParametersViewModel ChairParameters
        {
            get { return _boardVM.Chair.ChairParameters; }
        }

        public ObservableCollection<MemberParameterCollectionViewModel> Technicals
        {
            get { return _boardVM.Technicals; }
        }

        public ObservableCollection<MemberParameterCollectionViewModel> Legals
        {
            get { return _boardVM.Legals; }
        }

        public int FinishedCaseCount
        {
            get { return _simulation.FinishedCases.Count; }
        }
        #endregion



        #region Commands
        public ICommand AddMemberCommand
        { get { return new DelegateParamterisedCommand(_addMember); } }


        public ICommand RemoveMemberCommand
        { get { return new DelegateParamterisedCommand(_removeMember, _canRemoveMember); } }


        public ICommand ChangeBoardTypeCommand
        { get { return new DelegateParamterisedCommand(_changeBoardType); } }

        private void _addMember(object parameter)
        {
            ObservableCollection<MemberParameterCollectionViewModel> collection = 
                parameter as ObservableCollection<MemberParameterCollectionViewModel>;
            collection?.Add(new MemberParameterCollectionViewModel(__defaultTechnical()));           
        }

        private void _removeMember(object parameter)
        {
            ObservableCollection<MemberParameterCollectionViewModel> collection = 
                parameter as ObservableCollection<MemberParameterCollectionViewModel>;
            if (collection?.Count > 1)
            {
                collection.RemoveAt(0);
            }
        }

        private void _changeBoardType(object parameter)
        {
            switch (_boardVM.ChairType)
            {
                case ChairType.Technical:
                    _boardVM.ChairType = ChairType.Legal;
                    break;
                case ChairType.Legal:
                    _boardVM.ChairType = ChairType.Technical;
                    break;
            }
        }
        private bool _canRemoveMember(object parameter)
        {
            ObservableCollection<MemberParameterCollectionViewModel> collection =
                parameter as ObservableCollection<MemberParameterCollectionViewModel>;
            return collection?.Count > 1;
        }
        #endregion



        #region construction
        public SimulationViewModel()
        {
            BoardParameters boardParameters = new BoardParameters(
                __type,
                __chair,
                __technicals,
                __legals);

            _boardVM = new BoardParametersViewModel(boardParameters);

            _miniSim();

            _boardVM.PropertyChanged += (s, e) => _miniSim();
        }
        #endregion




        private void _miniSim()
        {
            _simulation = new Simulation(__miniSimulationLength, _boardVM.Parameters.AsSimulatorBoardParameters, __initialCaseCount);
            _simulation.Run();
            OnPropertyChanged("FinishedCaseCount");
            OnPropertyChanged("ChairType");
        }
    }
}