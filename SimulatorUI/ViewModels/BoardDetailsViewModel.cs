using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using SimulatorB;

namespace SimulatorUI
{
    public class BoardDetailsViewModel : ViewModel
    {
        #region Constants
        private const int __MinimumTechnicalCountForTechnicalChair = 1;
        private const int __MinimumTechnicalCountForLegalChair = 2;
        private const int __MimumLegalCountForTechnicalChair = 1;
        private const int __MinimumLegalCountForLegalchair = 0;

        private const int __MaximumMemberCount = 10;

        private const int __MaximumSecondaryChairWorkPercentage = 50;
        #endregion


        // TODO: move this out of here?
        #region Static
        private static MemberParameterCollection __defaultMember()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }

        #endregion
        

        #region Fields and Properties
        private BoardParameters _parameters;
        private ChairType _chairType;
        private int _remainingChairPercentage;



        private IEnumerable<MemberParameterCollection_DynamicViewModel> _members
        {
            get
            {
                foreach (var t in Technicals)
                    yield return t;
                foreach (var l in Legals)
                    yield return l;
            }
        }


        private int _minimumLegalCount
        {
            get => _chairType == ChairType.Technical 
                ? __MimumLegalCountForTechnicalChair 
                : __MinimumLegalCountForLegalchair;
        }

        private int _minimumTechnicalCount
        {
            get => _chairType == ChairType.Technical 
                ? __MinimumTechnicalCountForTechnicalChair
                : __MinimumTechnicalCountForLegalChair;
        }

        private int _secondaryChairPercentageTotal
        {
            get => Technicals.Sum(x => x.ChairWorkPercentage) + Legals.Sum(x => x.ChairWorkPercentage);
        }

        public int RemainingChairPercentage
        {
            get => _remainingChairPercentage;
            set => SetProperty(ref _remainingChairPercentage, value, "RemainingChairPercentage");
        }        

        private void _setRemainingChairPercentage()
        {
            RemainingChairPercentage = 100 - _secondaryChairPercentageTotal;
            Chair.ChairWorkPercentage = RemainingChairPercentage;
        }
        #endregion


        #region public VM
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
        { get { return new DelegateParamterisedCommand(_addTechnicalMember, _canAddTechnicalMember); } }

        public ICommand AddLegalMemberCommand
        { get { return new DelegateParamterisedCommand(_addLegalMember, _canAddLegallMember); } }                

        public ICommand RemoveMemberCommand
        { get { return new DelegateParamterisedCommand(_removeMember, _canRemoveMember); } }

        public ICommand ChangeBoardTypeCommand
        { get { return new DelegateParamterisedCommand(_changeBoardType, _canChangeBoardType); } }


        private void _removeMember(object parameter)
        {
            var memberToRemove = parameter as MemberParameterCollection_DynamicViewModel;

            if (Technicals.Count > _minimumTechnicalCount && Technicals.Contains(memberToRemove))
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
                (Technicals.Count > _minimumTechnicalCount && Technicals.Contains(memberToRemove))
                || (Legals.Count > _minimumLegalCount && Legals.Contains(memberToRemove));
        }


        private void _addTechnicalMember(object parameter)
        {
            MemberParameterCollection newMember = __defaultMember();
            Technicals.Add(new MemberParameterCollection_DynamicViewModel(
                newMember,
                Math.Min(__MaximumSecondaryChairWorkPercentage, RemainingChairPercentage)));
            _parameters.Technicals.Add(newMember);
        }

        private bool _canAddTechnicalMember(object obj)
        {
            return Technicals.Count < __MaximumMemberCount;
        }


        private void _addLegalMember(object parameter)
        {
            MemberParameterCollection newMember = __defaultMember();
            Legals.Add(
                new MemberParameterCollection_DynamicViewModel(
                    newMember,
                Math.Min(__MaximumSecondaryChairWorkPercentage, RemainingChairPercentage)));
            _parameters.Legals.Add(newMember);
        }

        private bool _canAddLegallMember(object obj)
        {
            return Legals.Count < __MaximumMemberCount;
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




        #region Construction
        public BoardDetailsViewModel(BoardParameters parameters)
        {
            _parameters = parameters;

            ChairType = parameters.ChairType;
            Chair = new MemberParameterCollection_DynamicViewModel(parameters.Chair, 100);
            Technicals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();
            Legals = new ObservableCollection<MemberParameterCollection_DynamicViewModel>();

            _remainingChairPercentage = 100;
            _setListeners();
            _assembleVMList(Technicals, parameters.Technicals);
            _assembleVMList(Legals, parameters.Legals);            
        }


        private void _assembleVMList(
            ObservableCollection<MemberParameterCollection_DynamicViewModel> collection,
            List<MemberParameterCollection> parametersList)
        {
            foreach (MemberParameterCollection mpc in parametersList)
            {
                collection.Add(
                    new MemberParameterCollection_DynamicViewModel(
                        mpc,
                Math.Min(__MaximumSecondaryChairWorkPercentage, RemainingChairPercentage)));
            }
        }
        #endregion



        #region Event handling
        private void _setListeners()
        {
            Technicals.CollectionChanged += (s, e) => _registerListeners("Technicals", s, e);
            Legals.CollectionChanged += (s, e) => _registerListeners("Legals", s, e);       
            
            Chair.PropertyChanged += (s, e) => OnPropertyChanged("Chair");
            Technicals.CollectionChanged += (s, e) => OnPropertyChanged("Technicals");
            Legals.CollectionChanged += (s, e) => OnPropertyChanged("Legals");
        }


        private void _registerListeners(
            string name,
            object sender,
            NotifyCollectionChangedEventArgs eventargs)
        {
            var collection = sender as ObservableCollection<MemberParameterCollection_DynamicViewModel>;
            if (collection == null)
                return;

            switch (eventargs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        mpc.PropertyChanged += (s, e) => _handleMemberChanges(name, s, e);
                        _updateRemainingChairPercentageAndNotify(mpc);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        mpc.PropertyChanged -= (s, e) => _handleMemberChanges(name, s, e);
                        _updateRemainingChairPercentageAndNotify(mpc);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.OldItems)
                    {
                        mpc.PropertyChanged -= (s, e) => _handleMemberChanges(name, s, e);
                        _updateRemainingChairPercentageAndNotify(mpc);
                    }
                    foreach (MemberParameterCollection_DynamicViewModel mpc in eventargs.NewItems)
                    {
                        mpc.PropertyChanged += (s, e) => _handleMemberChanges(name, s, e);
                        _updateRemainingChairPercentageAndNotify(mpc);

                    }
                    break;
                default:
                    break;
            }
        }

        private void _updateRemainingChairPercentageAndNotify(MemberParameterCollection_DynamicViewModel mpc)
        {
            if (mpc.ChairWorkPercentage != 0)
            {
                _setRemainingChairPercentage();
                _updateMembers(_members.Where(x => x != mpc));
            }
        }

        private void _handleMemberChanges (string name, object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "ChairWorkPercentage")
            {
                _setRemainingChairPercentage();
                _updateMembers(_members.Where(x => x != sender));
            }

            this.OnPropertyChanged(name);
        }
        

        private void _updateMembers(IEnumerable<MemberParameterCollection_DynamicViewModel> members)
        {
            foreach (var m in members)
                m.MaximumAvailableChairWorkPercentage = 
                    Math.Min(
                        __MaximumSecondaryChairWorkPercentage,
                        RemainingChairPercentage + m.ChairWorkPercentage);
        }
        #endregion
        

        
    }
}