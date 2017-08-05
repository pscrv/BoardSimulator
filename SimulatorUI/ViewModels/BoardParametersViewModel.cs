using System;
using System.Collections.Generic;
using System.ComponentModel;
using Simulator;

namespace SimulatorUI
{
    internal class BoardParametersViewModel : ObservableObject
    {
        #region fields and properties
        public ChairType ChairType { get; }
        public MemberParameterCollectionViewModel Chair { get; }
        public List<MemberParameterCollectionViewModel> Technicals { get; }
        public List<MemberParameterCollectionViewModel> Legals { get; }

        private int _finished;
        public int FinishedCaseCount
        {
            get { return _finished; }
            set
            {
                if (_finished != value)
                {
                    _finished = value;
                    OnPropertyChanged("FinishedCaseCount");
                }
            }
        }
        #endregion


        #region construction
        public BoardParametersViewModel(BoardParameters paremeters)
        {
            Chair = new MemberParameterCollectionViewModel(paremeters.Chair);
            Technicals = _assembleVMList(paremeters.Technicals);
            Legals = _assembleVMList(paremeters.Legals);

            Chair.PropertyChanged += (s,e) => this.OnPropertyChanged("Chair");
            foreach (MemberParameterCollectionViewModel mpc in Technicals)
            {
                mpc.PropertyChanged += (s, e) => this.OnPropertyChanged("Technicals");
            }
            foreach (MemberParameterCollectionViewModel mpc in Legals)
            {
                mpc.PropertyChanged += (s, e) => this.OnPropertyChanged("Legals");
            }
        }
        #endregion



        private List<MemberParameterCollectionViewModel> _assembleVMList(List<MemberParameterCollection> parametersList)
        {
            List<MemberParameterCollectionViewModel> result = new List<MemberParameterCollectionViewModel>();
            foreach (MemberParameterCollection collection in parametersList)
            {
                result.Add(new MemberParameterCollectionViewModel(collection));
            }

            return result;
        }
    }
}