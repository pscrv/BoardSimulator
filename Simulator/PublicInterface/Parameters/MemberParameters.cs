namespace Simulator
{
    public class MemberParameters
    {
        internal readonly int HoursForSummons;
        internal readonly int HoursOPPrepration;
        internal readonly int HoursForDecision;


        public MemberParameters(int summonsHours, int opPreparationHours, int decisionHours)
        {
            HoursForSummons = summonsHours;
            HoursOPPrepration = opPreparationHours;
            HoursForDecision = decisionHours;
        }
    }


    public class MemberParameterCollection
    {
        internal static MemberParameterCollection DefaultCollection()
        {
            MemberParameters chair = new MemberParameters(16, 4, 8);
            MemberParameters rapporteur = new MemberParameters(40, 8, 24);
            MemberParameters other = new MemberParameters(8, 4, 8);

            return new MemberParameterCollection(chair, rapporteur, other);
        }


        internal readonly MemberParameters ChairWorkParameters;
        internal readonly MemberParameters RapporteurWorkParameters;
        internal readonly MemberParameters OtherWorkParameters;


        public MemberParameterCollection(MemberParameters chair, MemberParameters rapporteur, MemberParameters other)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            OtherWorkParameters = other;
        }
    }

}