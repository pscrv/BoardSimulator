using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class AllocatedCase
    {
        #region fields and properties
        private OPSchedule _opSchedule = WorkQueues.OPSchedule;

        internal readonly AppealCase Case;
        internal readonly CaseBoard Board;
        internal readonly CaseRecord Record;

        internal CaseStage Stage
        {
            get
            {
                if (Record.RapporteurSummons.Finish == null
                    || Record.OtherMemberSummons.Finish == null
                    || Record.ChairSummons.Finish == null)
                    return CaseStage.Summons;

                if (Record.OP.Finish == null)
                    return CaseStage.OP;

                if (Record.RapporteurDecision.Finish == null
                    || Record.OtherMemberDecision.Finish == null
                    || Record.ChairDecision.Finish == null)
                    return CaseStage.Decision;

                return CaseStage.Finished;
            }
        }

        internal WorkType WorkType
        {
            get
            {
                switch (Stage)
                {
                    case CaseStage.Summons:
                        return WorkType.Summons;
                    case CaseStage.Decision:
                        return WorkType.Decision;

                    case CaseStage.OP:
                    case CaseStage.Finished:
                        break;
                }
                return WorkType.None;
            }
        }
        #endregion


        #region construction
        internal AllocatedCase(AppealCase ac, CaseBoard bd, Hour currentHour)
        {
            Case = ac;
            Board = bd;

            Record = new CaseRecord(ac);
            Record.SetAllocation(currentHour);            
        }
        #endregion


        internal void RecordStartOfWork(CaseWorker caseWorker, Hour currentHour)
        {
            WorkerRole role = caseWorker.Role;
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsStart(role, currentHour);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionStart(role, currentHour);
                    break;

                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.RecordStartOfWork: no summons or decision work to start.");
            }
        }


        internal void RecordFinishedWork(CaseWorker caseWorker, Hour currentHour)
        {
            WorkerRole role = caseWorker.Role;
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsFinish(role, currentHour);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionFinish(role, currentHour);
                    break;

                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.Recordwork: there is no work to do.");
            }
        }


        internal Member GetMemberByRole(WorkerRole role)
        {
            return Board.GetMemberByRole(role);
        }



        internal void EnqueueForWork(Hour currentHour)
        {
            if (_isFinished)
            {
                //TODO: decide what to do with finished cases. Possible nothing;
                return;
            }

            if (_isReadyForOP)
            {
                _opSchedule.Schedule(this);
                Record.SetOPEnqueue(currentHour);
                return;
            }


            WorkerRole role = Board.EnqueueForNextWorker(this);
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsEnqueue(role, currentHour);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionEnqueue(role, currentHour);
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.EnqueueForWork: Case is not in Summons or Decision stage.");
            
            }
        }

        private bool _isReadyForOP
        { get { return Record.ChairSummons.Finish != null && Record.OP.Enqueue == null; } }

        private bool _isFinished
        { get { return Record.ChairDecision.Finish != null; } }
    }
}