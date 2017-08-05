namespace SimulatorUI
{
    internal class MemberParametersViewModel : ObservableObject
    {
        private MemberParameters _parameters;

        public MemberParametersViewModel(MemberParameters parameters)
        {
            _parameters = parameters;
        }

        public int HoursForSummons
        {
            get { return _parameters.HoursForSummons; }
            set
            {
                if (_parameters.HoursForSummons != value)
                {
                    _parameters.HoursForSummons = value;
                    OnPropertyChanged("HoursForSummons");
                }
            }
        }

        public int HoursForOPPreparation
        {
            get { return _parameters.HoursOPPrepration; }
            set
            {
                if (_parameters.HoursOPPrepration != value)
                {
                    _parameters.HoursOPPrepration = value;
                    OnPropertyChanged("HoursForOPPreparation");}
            }
        }

        public int HoursForDecision
        {
            get { return _parameters.HoursForDecision; }
            set
            {
                if (_parameters.HoursForDecision != value)
                {
                    _parameters.HoursForDecision = value;
                    OnPropertyChanged("HoursForDecision");
                }
            }
        }
    }
}