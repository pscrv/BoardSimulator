using SimulatorB;

namespace SimulatorUI
{
    public class MemberParameters
    {
        public int HoursForSummons;
        public int HoursOPPrepration;
        public int HoursForDecision;


        public SimulatorB.MemberParameters AsSimulatorBParameters
        {
            get
            {
                return new SimulatorB.MemberParameters(HoursForSummons, HoursOPPrepration, HoursForDecision);
            }
        }


        public MemberParameters(int summonsHours, int opPreparationHours, int decisionHours)
        {
            HoursForSummons = summonsHours;
            HoursOPPrepration = opPreparationHours;
            HoursForDecision = decisionHours;
        }


        public MemberParameters Add(MemberParameters other)
        {
            return new MemberParameters(
                HoursForSummons + other.HoursForSummons,
                HoursOPPrepration + other.HoursOPPrepration,
                HoursForDecision + other.HoursForDecision);
        }

    }

    
}