namespace SimulatorOld
{
    internal class WorkReport
    {
        #region internal properties
        internal readonly AppealCase Case;
        internal readonly Work.WorkType Type;
        internal readonly Work.WorkRole Role;
        internal readonly Work.WorkState State;
        #endregion


        #region constructors
        internal WorkReport(AppealCase ac, Work.WorkType type, Work.WorkRole role, Work.WorkState state)
        {
            Case = ac;
            Type = type;
            Role = role;
            State = state;
        }
        #endregion
    }



    internal class OPWorkReport : WorkReport
    {
        public OPWorkReport(AppealCase ac) 
            : base(ac, Work.WorkType.OP, Work.WorkRole.None, Work.WorkState.None)
        { }
    }


    internal class NullWorkReport : WorkReport
    {
        internal NullWorkReport()
            : base (null, Work.WorkType.None, Work.WorkRole.None, Work.WorkState.None) 
        { }
    }
}