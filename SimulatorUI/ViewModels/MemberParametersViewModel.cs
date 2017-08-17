namespace SimulatorUI
{
    public class MemberParametersViewModel : ViewModel
    {
        private MemberParameters _parameters;


        public MemberParameters Parameters
        { get => Parameters; }
        
        public int HoursForSummons
        {
            get => _parameters.HoursForSummons; 
            set { SetProperty(ref _parameters.HoursForSummons, value, "HoursForSummons"); }
        }

        public int HoursForOPPreparation
        {
            get => _parameters.HoursOPPrepration; 
            set { SetProperty(ref _parameters.HoursOPPrepration, value, "HoursForOPPreparation"); }
        }

        public int HoursForDecision
        {
            get => _parameters.HoursForDecision; 
            set { SetProperty(ref _parameters.HoursForDecision, value, "HoursForDecision"); }
        }





        public MemberParametersViewModel(MemberParameters parameters)
        {
            _parameters = parameters;
        }
        



        public MemberParametersViewModel Add(MemberParametersViewModel other)
        {
            return new MemberParametersViewModel(
                _parameters.Add(other._parameters));
        }
    }



    public class MemberParameters_DynamicViewModel : MemberParametersViewModel
    {
        public MemberParameters_DynamicViewModel(MemberParameters parameters) 
            : base(parameters) { }
    }

    public class MemberParameters_FixedViewModel : MemberParametersViewModel
    {
        public MemberParameters_FixedViewModel(MemberParameters parameters)
            : base(parameters) { }
    }
}