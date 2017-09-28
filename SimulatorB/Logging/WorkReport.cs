namespace SimulatorB
{
    internal class WorkReport
    {
        internal readonly WorkType Type;
        internal readonly bool Finished;


        #region construction
        protected WorkReport(WorkType type, bool finished)
        {
            Type = type;
            Finished = finished;
        }


        internal WorkReport(SummonsCase workCase)
            : this (WorkType.Summons, workCase.MemberWorkIsFinished)
        { }

        internal WorkReport(OPCase workCase)
            : this(WorkType.OP, workCase.MemberWorkIsFinished)
        { }

        internal WorkReport(DecisionCase workCase)
            :this (WorkType.Decision, workCase.MemberWorkIsFinished)
        { }
        #endregion
    }


    internal class NullWorkReport : WorkReport
    {
        internal NullWorkReport()
            : base (WorkType.None, false)
        { }
    }
}
