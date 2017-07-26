namespace Simulator
{
    internal class MemberParameters
    {
        internal readonly int HoursForSummons;
        internal readonly int HoursOPPrepration;
        internal readonly int HoursForDecision;


        internal MemberParameters(int summonsHours, int opPreparationHours, int decisionHours)
        {
            HoursForSummons = summonsHours;
            HoursOPPrepration = opPreparationHours;
            HoursForDecision = decisionHours;
        }
    }


    internal class MemberParameterCollection
    {
        internal readonly MemberParameters ChairWorkParameters;
        internal readonly MemberParameters RapporteurWorkParameters;
        internal readonly MemberParameters OtherWorkParameters;


        internal MemberParameterCollection(MemberParameters chair, MemberParameters rapporteur, MemberParameters other)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            OtherWorkParameters = other;
        }
    }

}