namespace SimulatorB.PublicInterface
{
    public class FinishedCaseLog
    {
        private CaseLog _log;

        internal FinishedCaseLog(CaseLog log)
        {
            _log = log;
        }

        #region summons properties
        public int SummonsEnqueuedChair => _log.SummonsEnqueuedChair.Value;
        public int SummonsStartedChair => _log.SummonsStartedChair.Value;
        public int SummonsFinishedChair => _log.SummonsFinishedChair.Value;

        public int SummonsEnqueuedRapporteur => _log.SummonsEnqueuedRapporteur.Value;
        public int SummonsStartedRapporteur => _log.SummonsStartedRapporteur.Value;
        public int SummonsFinishedRapporteur => _log.SummonsFinishedRapporteur.Value;

        public int SummonsEnqueuedSecondMember => _log.SummonsEnqueuedSecondMember.Value;
        public int SummonsStartedSecondMember => _log.SummonsStartedRapporteur.Value;
        public int SummonsFinishedSecondMember => _log.SummonsFinishedRapporteur.Value;
        #endregion


        #region OP properties
        public int OPEnqueuedChair => _log.OPEnqueuedChair.Value;
        public int OPStartedChair => _log.OPStartedChair.Value;
        public int OPFinishedChair => _log.OPFinishedChair.Value;

        public int OPEnqueuedRapporteur => _log.OPEnqueuedRapporteur.Value;
        public int OPStartedRapporteur => _log.OPStartedRapporteur.Value;
        public int OPFinishedRapporteur => _log.OPStartedRapporteur.Value;

        public int OPEnqueuedSecondMember => _log.OPEnqueuedSecondMember.Value;
        public int OPStartedSecondMember => _log.OPStartedSecondMember.Value;
        public int OPFinishedSecondMember => _log.OPFinishedSecondMember.Value;
        #endregion


        #region Decision properties
        public int DecisionEnqueuedChair => _log.DecisionEnqueuedChair.Value;
        public int DecisionStartedChair => _log.DecisionStartedChair.Value;
        public int DecisionFinishedChair => _log.DecisionFinishedChair.Value;

        public int DecisionEnqueuedRapporteur => _log.DecisionEnqueuedRapporteur.Value;
        public int DecisionStartedRapporteur => _log.DecisionStartedRapporteur.Value;
        public int DecisionFinishedRapporteur => _log.DecisionFinishedRapporteur.Value;

        public int DecisionEnqueuedSecondMember => _log.DecisionEnqueuedSecondMember.Value;
        public int DecisionStartedSecondMember => _log.DecisionStartedSecondMember.Value;
        public int DecisionFinishedSecondMember => _log.DecisionStartedSecondMember.Value;
        #endregion


        #region Finished properties
        public int Finished => _log.Finished.Value;
        #endregion
    }
}