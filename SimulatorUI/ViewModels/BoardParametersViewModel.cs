using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Simulator;

namespace SimulatorUI
{
    public class BoardParametersViewModel : ObservableObject
    {
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