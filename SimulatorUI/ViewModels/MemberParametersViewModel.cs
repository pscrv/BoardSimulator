namespace SimulatorUI
{
    public class MemberParametersViewModel : ObservableObject
    {
        private MemberParameters _parameters;

        public MemberParametersViewModel(MemberParameters parameters)
        {
            _parameters = parameters;
        }
        

        public int HoursForSummons
        {
            get { return _parameters.HoursForSummons; }
            set { SetProperty(ref _parameters.HoursForSummons, value, "HoursForSummons"); }
        }

        public int HoursForOPPreparation
        {
            get { return _parameters.HoursOPPrepration; }
            set { SetProperty(ref _parameters.HoursOPPrepration, value, "HoursForOPPreparation"); }
        }

        public int HoursForDecision
        {
            get { return _parameters.HoursForDecision; }
            set { SetProperty(ref _parameters.HoursForDecision, value, "HoursForDecision"); }
        }
    }
}