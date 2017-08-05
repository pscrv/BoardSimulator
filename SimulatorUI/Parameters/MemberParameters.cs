namespace SimulatorUI
{
    public class MemberParameters
    {
        public int HoursForSummons;
        public int HoursOPPrepration;
        public int HoursForDecision;


        public Simulator.MemberParameters AsSimulatorParameters
        {
            get
            {
                return new Simulator.MemberParameters(HoursForSummons, HoursOPPrepration, HoursForDecision);
            }
        }


        public MemberParameters(int summonsHours, int opPreparationHours, int decisionHours)
        {
            HoursForSummons = summonsHours;
            HoursOPPrepration = opPreparationHours;
            HoursForDecision = decisionHours;
        }
    }


    public class MemberParameterCollection
    {
        public static MemberParameterCollection DefaultCollection()
        {
            MemberParameters chair = new MemberParameters(16, 4, 8);
            MemberParameters rapporteur = new MemberParameters(40, 8, 24);
            MemberParameters other = new MemberParameters(8, 4, 8);

            return new MemberParameterCollection(chair, rapporteur, other);
        }



        public MemberParameters ChairWorkParameters;
        public MemberParameters RapporteurWorkParameters;
        public MemberParameters OtherWorkParameters;



        public Simulator.MemberParameterCollection AsSimulatorParameters
        {
            get
            {
                return new Simulator.MemberParameterCollection(
                    ChairWorkParameters.AsSimulatorParameters,
                    RapporteurWorkParameters.AsSimulatorParameters,
                    OtherWorkParameters.AsSimulatorParameters);
            }
        }



        public MemberParameterCollection(MemberParameters chair, MemberParameters rapporteur, MemberParameters other)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            OtherWorkParameters = other;
        }
    }

}