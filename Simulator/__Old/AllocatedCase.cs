namespace OldSim
{
    internal class AllocatedCase
    {
        private AppealCase _case;
        private CaseBoard _caseBoard;
        private CaseWork _summonsWork;
        private CaseWork _decisionWork;
        
        
        internal int CaseID { get { return _case.ID; } }
        internal bool IsInSummonsStage { get { return _summonsWork.ChairState != WorkState.Finished; } }
        internal bool IsInDecisionStage
        {
            get
            {
                return 
                    _summonsWork.ChairState == WorkState.Finished 
                    && _decisionWork.ChairState != WorkState.Finished;
            }
        }


        internal AllocatedCase(AppealCase ac, CaseBoard cb)
        {
            _case = ac;
            _caseBoard = cb;
            _summonsWork = new CaseWork(
                _caseBoard.Rapporteur.HoursPerSummons,
                _caseBoard.Chair.HoursPerSummons,
                _caseBoard.Other.HoursPerSummons);
            _decisionWork = new CaseWork(
                _caseBoard.Rapporteur.HoursPerDecision,
                _caseBoard.Chair.HoursPerDecision,
                _caseBoard.Other.HoursPerDecision);
        }





        public override string ToString()
        {
            return string.Format("AllocatedCase <{0}>", CaseID);
        }


    }
}
