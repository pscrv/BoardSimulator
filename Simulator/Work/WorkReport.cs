namespace Simulator
{
    internal class WorkReport
    {
        #region static constructors
        internal static WorkReport MakeReport(AppealCase ac, WorkType type, WorkerRole role, WorkState state)
        {
            return new WorkReport(ac, type, role, state);
        }

        internal static WorkReport MakeNullReport()
        {
            return new WorkReport(null, WorkType.None, WorkerRole.None, WorkState.None);
        }

        internal static WorkReport MakeOPReport(AppealCase ac, WorkerRole role)
        {
            return new WorkReport(ac, WorkType.OP, role, WorkState.None);
        }
        #endregion



        #region internal properties
        internal readonly AppealCase Case;
        internal readonly WorkType Type;
        internal readonly WorkerRole Role;
        internal readonly WorkState State;
        #endregion


        #region constructors
        private WorkReport(AppealCase ac, WorkType type, WorkerRole role, WorkState state)
        {
            Case = ac;
            Type = type;
            Role = role;
            State = state;
        }
        #endregion
    }
}