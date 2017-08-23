using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Simulator;

namespace SimulatorUI
{
    public class BoardDetailsViewModel : ViewModel
    {
        #region constants
        private const int __MinimumTechnicalCountForTechnicalChair = 1;
        private const int __MinimumTechnicalCountForLegalChair = 2;
        private const int __MimumLegalCountForTechnicalChair = 1;
        private const int __MinimumLegalCountForLegalchair = 0;
        #endregion

        #region static
        // TODO: move this out of here?
        private static MemberParameterCollection __defaultTechnicalMember()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }

        #endregion



        #region fields and properties
        private BoardParameters _parameters;

        private ChairType _chairType;

        private int _minimumLegalCount
        {
            get => _chairType == ChairType.Technical 
                ? __MimumLegalCountForTechnicalChair 
                : __MinimumLegalCountForLegalchair;
        }

        private int _minimamTechnicalCount
        {
            get => _chairType == ChairType.Technical 
                ? __MinimumTechnicalCountForTechnicalChair
                : __MinimumTechnicalCountForLegalChair;
        }

        public ChairType ChairType
        {
            get { return _chairType; }
            set { SetProperty(ref _chairType, value, "ChairType"); }
        }
        public MemberParameterCollection_DynamicViewModel Chair { get; }
        public ObservableCollection<MemberParameterCollection_DynamicViewModel> Technicals { get; }
        public ObservableCollection<MemberParameterCollection_DynamicViewModel> Legals { get; }

        public BoardParameters Parameters { get => _parameters; }
        #endregion



        #region Commands
        public ICommand AddTechnicalMemberCommand
        { get { return new DelegateParamterisedCommand(_addTechnicalMember); } }
                
        public ICommand AddLegalMemberCommand
        { get { return new DelegateParamterisedCommand(_addLegalMember); } }                

        public ICommand RemoveMemberCommand
        { get { return new DelegateParamterisedCommand(_removeMember, _canRemoveMember); } }

        public ICommand ChangeBoardTypeCommand
        { get { return new DelegateParamterisedCommand(_changeBoardType, _canChangeBoardType); } }


        private void _removeMember(object parameter)
        {
            var memberToRemove = parameter as MemberParameterCollection_DynamicViewModel;

            if (Technicals.Count > _minimamTechnicalCount && Technicals.Contains(memberToRemove))
            {
                Technicals.Remove(memberToRemove);
                _parameters.Technicals.Remove(memberToRemove.Parameters);
                return;
            }

            if (Legals.Count > _minimumLegalCount && Legals.Contains(memberToRemove))
            {
                Legals.Remove(memberToRemove);
                _parameters.Legals.Remove(memberToRemove.Parameters);
                return;
            }

            throw new InvalidOperationException("Attempt to remove a non-member.");
        }

        private bool _canRemoveMember(object parameter)
        {
            var memberToRemove = parameter as MemberParameterCollection_DynamicViewModel;

            return 
                (Technicals.Count > _minimamTechnicalCount && Technicals.Contains(memberToRemove))
                || (Legals.Count > _minimumLegalCount && Legals.Contains(memberToRemove));
        }


        private void _addTechnicalMember(object parameter)
        {
            MemberParameterCollection newMember = __defaultTechnicalMember();
            Technicals.Add(new MemberParameterCollection_DynamicViewModel(newMember));
            _parameters.Technicals.Add(newMember);
        }

        private void _addLegalMember(object parameter)
        {
            MemberParameterCollection newMember = __defaultTechnicalMember();
            Legals.Add(new MemberParameterCollection_DynamicViewModel(newMember));
            _parameters.Legals.Add(newMember);
        }
        

        private void _changeBoardType(object parameter)
        {
            switch (ChairType)
            {
                case ChairType.Technical:
                    ChairType = ChairType.Legal;
                    _parameters.ChairType = ChairType.Legal;
                    break;
                case ChairType.Legal:
                    ChairType = ChairType.Technical;
                    _parameters.ChairType = ChairType.Technical;
                    break;
            }
        }
        
        private bool _canChangeBoardType(object obj)
        {
            switch (_chairType)
            {
                case ChairType.Technical:
                    return Technicals.Count >= __MinimumTechnicalCountForLegalChair
                        && Legals.Count >= __MinimumLegalCountForLegalchair;

                case ChairType.Legal:
                    return Technicals.Count >= __MinimumTechnicalCountForTechnicalChair
                        && Legals.Count >= __MimumLegalCountForTechnicalChair;
            }

            return false;
        }
        #endregion




        #region construction
        public BoardDetailsViewModel(BoardParameters parameters)
        {
            _parameters = parameters;

            ChairType = parameters.ChairType;
            Chair = new MemberParameterCollection_DynamicViewModel(parameters.Chair);
            Technicals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();
            Legals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();

            _setListeners();

            _assembleVMList(Technicals, parameters.Technicals);
            _assembleVMList(Legals, parameters.Legals);
        }

        private void _setListeners()
        {
            Chair.PropertyChanged += (s, e) => OnPropertyChanged("Chair");
            Technicals.CollectionChanged += (s, e) => OnPropertyChanged("Technicals");
            Technicals.CollectionChanged += (s, e) => _registerListeners("Technicals", s, e);
            Legals.CollectionChanged += (s, e) => OnPropertyChanged("Legals");
            Legals.CollectionChanged += (s, e) => _registerListeners("Legals", s, e);
        }


        private void _registerListeners(
            string name,
            object sender,
            NotifyCollectionChangedEventArgs eventargs)
        {
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection = sender as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
            if (collection == null)
                return;

            switch (eventargs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                default:
                    break;
            }
        }


        private void _addListener(MemberParameterCollection_DynamicViewModel mpc, string name)
        {
            mpc.PropertyChanged += (s, e) => this.OnPropertyChanged(name);
        }


        private void _removeListener(MemberParameterCollection_DynamicViewModel mpc, string name)
        {
            mpc.PropertyChanged -= (s, e) => this.OnPropertyChanged(name);
        }
        #endregion


        private void _assembleVMList(
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection,
            List<MemberParameterCollection> parametersList)
        {
            foreach (MemberParameterCollection mpc in parametersList)
            {
                collection.Add(new MemberParameterCollection_DynamicViewModel(mpc));
            }
        }

    }









    public class aBoardDetailsViewModel : ViewModel
    {
        #region static

        // TODO: move this out of here?
        // TODO: different defaults for technical/legal
        private static MemberParameterCollection __defaultMember()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }
        
        #endregion



        #region fields and properties
        private ChairType _chairType;

        public ChairType ChairType
        {
            get { return _chairType; }
            set { SetProperty(ref _chairType, value, "ChairType"); }
        }
        public MemberParameterCollection_DynamicViewModel Chair { get; }
        public ObservableCollection<MemberParameterCollection_DynamicViewModel> Technicals { get; }
        public ObservableCollection<MemberParameterCollection_DynamicViewModel> Legals { get; }    
        
        public BoardParameters Parameters
        {
            get
            {
                List<MemberParameterCollection> technicals = new List<MemberParameterCollection>();
                List<MemberParameterCollection> legals = new List<MemberParameterCollection>();
                
                foreach (MemberParameterCollection_DynamicViewModel mpc in Technicals)
                {
                    technicals.Add(mpc.Parameters);
                }

                foreach (MemberParameterCollection_DynamicViewModel mpc in Legals)
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
        // TODO: AddMemeberCommand should use different defaults for technical/legal members
        public ICommand AddMemberCommand
        { get { return new DelegateParamterisedCommand(_addMember); } }


        public ICommand RemoveMemberCommand
        { get { return new DelegateParamterisedCommand(_removeMember, _canRemoveMember); } }


        public ICommand ChangeBoardTypeCommand
        { get { return new DelegateParamterisedCommand(_changeBoardType); } }

        private void _addMember(object parameter)
        {
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection =
                parameter as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
            collection?.Add(new MemberParameterCollection_DynamicViewModel(__defaultMember()));
        }

        private void _removeMember(object parameter)
        {
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection =
                parameter as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
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
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection =
                parameter as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
            return collection?.Count > 1;
        }
        #endregion



        #region construction
        public aBoardDetailsViewModel(BoardParameters parameters)
        {
            ChairType = parameters.ChairType;
            Chair = new MemberParameterCollection_DynamicViewModel(parameters.Chair);
            Technicals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();
            Legals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();

            _setListeners();

            _assembleVMList(Technicals, parameters.Technicals);
            _assembleVMList(Legals, parameters.Legals);
        }

        private void _setListeners()
        {
            Chair.PropertyChanged += (s, e) => OnPropertyChanged("Chair");
            Technicals.CollectionChanged += (s, e) => OnPropertyChanged("Technicals");
            Technicals.CollectionChanged += (s, e) => _registerListeners("Technicals", s, e);
            Legals.CollectionChanged += (s, e) => OnPropertyChanged("Legals");
            Legals.CollectionChanged += (s, e) => _registerListeners("Legals", s, e);
        }
        

        private void _registerListeners(
            string name,
            object sender,
            NotifyCollectionChangedEventArgs eventargs)
        {
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection = sender as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
            if (collection == null)
                return;

            switch (eventargs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        _removeListener(mpc, name);
                    }
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        _addListener(mpc, name);
                    }
                    break;
                default:
                    break;
            }
        }
        

        private void _addListener(MemberParameterCollection_DynamicViewModel mpc, string name)
        {
            mpc.PropertyChanged += (s, e) => this.OnPropertyChanged(name);
        }


        private void _removeListener(MemberParameterCollection_DynamicViewModel mpc, string name)
        {
            mpc.PropertyChanged -= (s, e) => this.OnPropertyChanged(name);
        }
        #endregion


        private void _assembleVMList(
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection,
            List<MemberParameterCollection> parametersList)
        {
            foreach (MemberParameterCollection mpc in parametersList)
            {
                collection.Add(new MemberParameterCollection_DynamicViewModel(mpc));
            }
        }
        
    }
}