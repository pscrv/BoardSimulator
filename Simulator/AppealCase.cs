namespace Simulator
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


        //internal void SetSummonsEnqueued()
        //{
        //    _state.SetSummonsEnqueued();
        //}

        //internal void SetSummonsStarted()
        //{
        //    _state.SetSummonsStarted();
        //}

        //internal void SetSummonsFinisheded()
        //{
        //    _state.SetSummonsFinisheded();
        //}

        //internal void SetOPStarted()
        //{
        //    _state.SetOPStarted();
        //}

        //internal void SetOPFinisheded()
        //{
        //    _state.SetOPFinisheded();
        //}

        //internal void SetDecisionEnqueued()
        //{
        //    _state.SetDecisionEnqueued();
        //}

        //internal void SetDecisionStarted()
        //{
        //    _state.SetDecisionStarted();
        //}

        //internal void SetDecisionFinished()
        //{
        //    _state.SetDecisionFinished();
        //}
        #endregion



        #region overrides
        public override string ToString()
        {
            return string.Format("CaseNumber <{0}>", ID);
        }
        #endregion
    }
}