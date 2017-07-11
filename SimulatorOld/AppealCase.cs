namespace SimulatorOld
{
    internal class AppealCase
    {
        #region static
        private static int __instanceCounter = 0;
        #endregion


        #region private fields
        private AppealCaseState _state;
        #endregion


        #region internal fields and properties
        internal readonly int ID;
        internal AppealCaseState.Stage Stage { get { return _state.CurrentStage; } }
        #endregion


        #region constructors
        internal AppealCase()
        {
            ID = __instanceCounter;
            __instanceCounter++;
            _state = new AppealCaseState();
        }
        #endregion


        #region internal methods
        internal void AdvanceState()
        {
            _state.Advance();
        }
        #endregion



        #region overrides
        public override string ToString()
        {
            return string.Format("CaseNumber <{0}>", ID);
        }
        #endregion
    }
}