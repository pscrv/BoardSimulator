namespace Simulator
{
    public class WorkReport
    {
        #region static constructors
        internal static WorkReport MakeReport(AppealCase ac, CaseStage stage, WorkerRole role, WorkState state)
        {
            return new WorkReport(ac, stage, role, state);
        }

        internal static WorkReport MakeNullReport()
        {
            return new WorkReport(null, CaseStage.Undefined, WorkerRole.None, WorkState.None);
        }

        internal static WorkReport MakeOPReport(AppealCase ac, WorkerRole role)
        {
            return new WorkReport(ac, CaseStage.OP, role, WorkState.None);
        }
        #endregion


        #region internal properties
        internal readonly AppealCase Case;
        internal readonly CaseStage Stage;
        internal readonly WorkerRole Role;
        internal readonly WorkState State;
        #endregion


        #region constructors
        private WorkReport(AppealCase ac, CaseStage stage, WorkerRole role, WorkState state)
        {
            Case = ac;
            Stage = stage;
            Role = role;
            State = state;
        }
        #endregion



        #region public interface
        public int AppealCase { get { return Case.ID; } }
        public CaseStage CaseStage { get { return Stage; } }
        public WorkerRole WorkerRole { get { return Role; } }
        public WorkState WorkState { get { return State; } }
        #endregion
    }
}