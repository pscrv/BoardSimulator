namespace OldSim
{
    internal class WorkReport
    {
        #region static constructors
        internal static WorkReport MakeReport(AppealCase ac, Work.WorkType type, Work.WorkRole role, Work.WorkState state)
        {
            return new WorkReport(ac, type, role, state);
        }

        internal static WorkReport MakeNullReport()
        {
            return new WorkReport(null, Work.WorkType.None, Work.WorkRole.None, Work.WorkState.None);
        }

        internal static WorkReport MakeOPReport(AppealCase ac, Work.WorkRole role, Work.WorkState state)
        {
            return new WorkReport(ac, Work.WorkType.OP, role, state);
        }
        #endregion



        #region internal properties
        internal readonly AppealCase Case;
        internal readonly Work.WorkType Type;
        internal readonly Work.WorkRole Role;
        internal readonly Work.WorkState State;
        #endregion


        #region constructors
        private WorkReport(AppealCase ac, Work.WorkType type, Work.WorkRole role, Work.WorkState state)
        {
            Case = ac;
            Type = type;
            Role = role;
            State = state;
        }
        #endregion
    }    
}