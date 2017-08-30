namespace SimulatorUI
{

    public class MemberParameterCollection
    {
        #region static
        public static MemberParameterCollection DefaultCollection()
        {
            return new MemberParameterCollection(
                new MemberParameters(16, 4, 8),
                new MemberParameters(40, 8, 24),
                new MemberParameters(8, 4, 8));
        }
        #endregion


        public readonly MemberParameters ChairWorkParameters;
        public readonly MemberParameters RapporteurWorkParameters;
        public readonly MemberParameters OtherWorkParameters;
        public int ChairWorkPercentage { get; set; }


        public Simulator.MemberParameterCollection AsSimulatorParameters
        {
            get
            {
                return new Simulator.MemberParameterCollection(
                    ChairWorkParameters.AsSimulatorParameters,
                    RapporteurWorkParameters.AsSimulatorParameters,
                    OtherWorkParameters.AsSimulatorParameters,
                    ChairWorkPercentage);
            }
        }



        public MemberParameterCollection(
            MemberParameters chair, 
            MemberParameters rapporteur, 
            MemberParameters other,
            int chairWorkPercentage = 0)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            OtherWorkParameters = other;
            ChairWorkPercentage = chairWorkPercentage;
        }
    }
}
