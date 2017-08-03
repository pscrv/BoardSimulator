using System.Collections.Generic;

namespace Simulator
{
    public class CompletedCaseReport
    {
        #region fields and properties
        public readonly int CaseID;
        public readonly int HourOfCreation;
        public readonly int HourOfAlloction;


        public readonly int ChairID;
        public readonly int RapporteurID;
        public readonly int OtherMemberID;

        public readonly int HourOPScheduled;
        public readonly int HourOPStarted;
        public readonly int HourOPFinished;
            
        private Dictionary<WorkerRole, int> _memberIDs = new Dictionary<WorkerRole, int>();

        private Dictionary<WorkerRole, int> _hourEnqueuedForSummons = new Dictionary<WorkerRole, int>();
        private Dictionary<WorkerRole, int> _hourSummonsWorkStarted = new Dictionary<WorkerRole, int>();
        private Dictionary<WorkerRole, int> _hourSummonsWorkFinished = new Dictionary<WorkerRole, int>();

        private Dictionary<WorkerRole, int> _hourEnqueuedForDecision = new Dictionary<WorkerRole, int>();
        private Dictionary<WorkerRole, int> _hourDecsionWorkStarted = new Dictionary<WorkerRole, int>();
        private Dictionary<WorkerRole, int> _hourDecisionWorkFinished = new Dictionary<WorkerRole, int>();
        #endregion


        #region construction
        internal CompletedCaseReport(AllocatedCase allocatedCase)
        {
            CaseID = allocatedCase.Case.ID;
            HourOfCreation = allocatedCase.Case.CreationHour.Value;
            HourOfAlloction = allocatedCase.Record.Allocation.Value;

            ChairID = allocatedCase.Board.Chair.ID;
            RapporteurID = allocatedCase.Board.Rapporteur.ID;
            OtherMemberID = allocatedCase.Board.OtherMember.ID;

            _memberIDs[WorkerRole.Chair] = allocatedCase.Board.Chair.ID;
            _memberIDs[WorkerRole.Rapporteur] = allocatedCase.Board.Chair.ID;
            _memberIDs[WorkerRole.OtherMember] = allocatedCase.Board.OtherMember.ID;
            
            _hourEnqueuedForSummons[WorkerRole.Chair] = allocatedCase.Record.ChairSummons.Enqueue.Value;
            _hourEnqueuedForSummons[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurSummons.Enqueue.Value;
            _hourEnqueuedForSummons[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberSummons.Enqueue.Value;

            _hourSummonsWorkStarted[WorkerRole.Chair] = allocatedCase.Record.ChairSummons.Start.Value;
            _hourSummonsWorkStarted[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurSummons.Start.Value;
            _hourSummonsWorkStarted[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberSummons.Start.Value;

            _hourSummonsWorkFinished[WorkerRole.Chair] = allocatedCase.Record.ChairSummons.Finish.Value;
            _hourSummonsWorkFinished[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurSummons.Finish.Value;
            _hourSummonsWorkFinished[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberSummons.Finish.Value;

            HourOPScheduled = allocatedCase.Record.OP.Enqueue.Value;
            HourOPStarted = allocatedCase.Record.OP.Start.Value;
            HourOPFinished = allocatedCase.Record.OP.Finish.Value;

            _hourEnqueuedForDecision[WorkerRole.Chair] = allocatedCase.Record.ChairDecision.Enqueue.Value;
            _hourEnqueuedForDecision[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurDecision.Enqueue.Value;
            _hourEnqueuedForDecision[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberDecision.Enqueue.Value;

            _hourDecsionWorkStarted[WorkerRole.Chair] = allocatedCase.Record.ChairDecision.Start.Value;
            _hourDecsionWorkStarted[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurDecision.Start.Value;
            _hourDecsionWorkStarted[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberDecision.Start.Value;

            _hourDecisionWorkFinished[WorkerRole.Chair] = allocatedCase.Record.ChairDecision.Finish.Value;
            _hourDecisionWorkFinished[WorkerRole.Rapporteur] = allocatedCase.Record.RapporteurDecision.Finish.Value;
            _hourDecisionWorkFinished[WorkerRole.OtherMember] = allocatedCase.Record.OtherMemberDecision.Finish.Value;
        }
        #endregion



        #region public interface
        public int HourEnqueuedForSummons(WorkerRole role) { return _hourEnqueuedForSummons[role]; }
        public int HourSummonsWorkStarted(WorkerRole role) { return _hourSummonsWorkStarted[role]; }
        public int HourSummonsWorkFinished(WorkerRole role) { return _hourSummonsWorkFinished[role]; }
        

        public int HourEnqueuedForDecision(WorkerRole role) { return _hourEnqueuedForDecision[role]; }
        public int HourDecisionWorkStarted(WorkerRole role) { return _hourDecsionWorkStarted[role]; }
        public int HourDecisionWorkFinished(WorkerRole role) { return _hourDecisionWorkFinished [role]; }
        #endregion



        #region overrides
        public override string ToString()
        {
            return string.Format("CompletedCaseReport <{0}>", CaseID);
        }
        #endregion
    }
}