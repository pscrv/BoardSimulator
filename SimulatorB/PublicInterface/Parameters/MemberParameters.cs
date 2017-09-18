namespace SimulatorB
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
        internal static MemberParameterCollection DefaultCollection
        {
            get
            {
                MemberParameters chair = new MemberParameters(16, 4, 8);
                MemberParameters rapporteur = new MemberParameters(40, 8, 24);
                MemberParameters second = new MemberParameters(8, 4, 8);

                return new MemberParameterCollection(chair, rapporteur, second);
            }
        }

        internal static MemberParameterCollection FastCollection
        {
            get
            {
                MemberParameters chair = new MemberParameters(1, 1, 1);
                MemberParameters rapporteur = new MemberParameters(1, 1, 1);
                MemberParameters second = new MemberParameters(1, 1, 1);

                return new MemberParameterCollection(chair, rapporteur, second);
            }
        }


        internal readonly MemberParameters ChairWorkParameters;
        internal readonly MemberParameters RapporteurWorkParameters;
        internal readonly MemberParameters SecondWorkParameters;
        internal readonly int ChairWorkPercentage;  //TODO: should this be here?


        public MemberParameterCollection(
            MemberParameters chair, 
            MemberParameters rapporteur, 
            MemberParameters second,
            int chairWorkPercentage = 0)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            SecondWorkParameters = second;
            ChairWorkPercentage = chairWorkPercentage;
        }
    }

}