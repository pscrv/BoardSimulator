namespace SimulatorUI
{
    public class BoardSummaryViewModel : ViewModel
    {
        #region fields and properties
        private BoardParameters _boardParameters;
        private MemberParameterCollection_FixedViewModel _averageParameterCollection;
        
        

        public MemberParameterCollection_FixedViewModel Average { get => _averageParameterCollection; }
        #endregion


        #region construction
        public BoardSummaryViewModel(BoardParameters boardParameters)
        {
            _boardParameters = boardParameters;
            
            int totalMembers = _boardParameters.Technicals.Count + _boardParameters.Legals.Count;
            int totalMembers_1 = 1 + totalMembers;


            MemberParameters chP = _boardParameters.Chair.ChairWorkParameters;
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Technicals)
            {
                chP = chP.Add(paremeterCollection.ChairWorkParameters);
            }
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Legals)
            {
                chP = chP.Add(paremeterCollection.ChairWorkParameters);
            }
            chP = new MemberParameters(
                chP.HoursForSummons / totalMembers_1,
                chP.HoursOPPrepration / totalMembers_1,
                chP.HoursForDecision / (totalMembers_1));

            MemberParameters rpP = new MemberParameters(0, 0, 0);
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Technicals)
            {
                rpP = rpP.Add(paremeterCollection.RapporteurWorkParameters);
            }
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Legals)
            {
                rpP = rpP.Add(paremeterCollection.RapporteurWorkParameters);
            }
            rpP = new MemberParameters(
                rpP.HoursForSummons / totalMembers,
                rpP.HoursOPPrepration / totalMembers,
                rpP.HoursForDecision / totalMembers);

            MemberParameters otP = new MemberParameters(0, 0, 0);
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Technicals)
            {
                otP = otP.Add(paremeterCollection.OtherWorkParameters);
            }
            foreach (MemberParameterCollection paremeterCollection in _boardParameters.Legals)
            {
                otP = otP.Add(paremeterCollection.OtherWorkParameters);
            }

            otP = new MemberParameters(
                otP.HoursForSummons / totalMembers,
                otP.HoursOPPrepration / totalMembers,
                otP.HoursForDecision / totalMembers);


            _averageParameterCollection = new MemberParameterCollection_FixedViewModel(
                new MemberParameterCollection(chP, rpP, otP));
        }
        #endregion
    }
}