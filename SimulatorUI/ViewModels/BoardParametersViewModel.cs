using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Simulator;

namespace SimulatorUI
{
    public class BoardParametersViewModel : ObservableObject
    {
        #region static
        private static ChairType __type = ChairType.Technical;
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


        public static BoardParametersViewModel MakeDefaultBoard()
        {
            BoardParameters boardParameters = new BoardParameters(
                __type,
                __chair,
                __technicals,
                __legals);

            return new BoardParametersViewModel(boardParameters);
        }
        #endregion



        #region fields and properties
        private ChairType _chairType;

        public ChairType ChairType
        {
            get { return _chairType; }
            set { SetProperty(ref _chairType, value, "ChairType"); }
        }
        public MemberParameterCollectionViewModel Chair { get; }
        public ObservableCollection<MemberParameterCollectionViewModel> Technicals { get; }
        public ObservableCollection<MemberParameterCollectionViewModel> Legals { get; }    
        
        public BoardParameters Parameters
        {
            get
            {
                List<MemberParameterCollection> technicals = new List<MemberParameterCollection>();
                List<MemberParameterCollection> legals = new List<MemberParameterCollection>();

                foreach (MemberParameterCollectionViewModel mpc in Technicals)
                {
                    technicals.Add(mpc.Parameters);
                }

                foreach (MemberParameterCollectionViewModel mpc in Legals)
                {
                    legals.Add(mpc.Parameters);
                }

                return new BoardParameters(
                    ChairType,
                    Chair.Parameters,
                    technicals,
                    legals);
            }
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
            switch (ChairType)
            {
                case ChairType.Technical:
                    ChairType = ChairType.Legal;
                    break;
                case ChairType.Legal:
                    ChairType = ChairType.Technical;
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
        public BoardParametersViewModel(BoardParameters parameters)
        {
            ChairType = parameters.ChairType;
            Chair = new MemberParameterCollectionViewModel(parameters.Chair);
            Technicals = new ObservableCollection<MemberParameterCollectionViewModel>();
            Legals = new ObservableCollection<MemberParameterCollectionViewModel>();

            _setListeners();

            _assembleVMList(Technicals, parameters.Technicals);
            _assembleVMList(Legals, parameters.Legals);
        }

        private void _setListeners()
        {
            Chair.PropertyChanged += (s, e) => this.OnPropertyChanged("Chair");
            Technicals.CollectionChanged += (s, e) => this.OnPropertyChanged("Technicals");
            Technicals.CollectionChanged += (s, e) => _registerListeners("Technicals", s, e);
            Legals.CollectionChanged += (s, e) => this.OnPropertyChanged("Legals");
            Legals.CollectionChanged += (s, e) => _registerListeners("Legals", s, e);
        }
        

        private void _registerListeners(
            string name,
            object sender,
            NotifyCollectionChangedEventArgs eventargs)
        {
            ObservableCollection<MemberParameterCollectionViewModel> collection = sender as ObservableCollection<MemberParameterCollectionViewModel>;
            if (collection == null)
                return;

            switch (eventargs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (MemberParameterCollectionViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (MemberParameterCollectionViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (MemberParameterCollectionViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    foreach (MemberParameterCollectionViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                default:
                    break;
            }
        }
        

        private void _addListener(MemberParameterCollectionViewModel mpc, string name)
        {
            mpc.PropertyChanged += (s, e) => this.OnPropertyChanged(name);
        }


        private void _removeListener(MemberParameterCollectionViewModel mpc, string name)
        {
            mpc.PropertyChanged -= (s, e) => this.OnPropertyChanged(name);
        }
        #endregion


        private void _assembleVMList(
            ObservableCollection<MemberParameterCollectionViewModel> collection,
            List<MemberParameterCollection> parametersList)
        {
            foreach (MemberParameterCollection mpc in parametersList)
            {
                collection.Add(new MemberParameterCollectionViewModel(mpc));
            }
        }
        
    }
}