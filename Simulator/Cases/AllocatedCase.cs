using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class AllocatedCase
    {
        #region fields and properties
        private OPQueue _opQueue = WorkQueues.OP;

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
        internal AllocatedCase(AppealCase ac, CaseBoard bd)
        {
            Case = ac;
            Board = bd;

            Record = new CaseRecord(ac);
            Record.SetAllocation();            
        }
        #endregion


        internal void RecordStartOfWork(CaseWorker caseWorker)
        {
            WorkerRole role = caseWorker.Role;
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsStart(role);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionStart(role);
                    break;

                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.RecordStartOfWork: no summons or decision work to start.");
            }
        }


        internal void RecordFinishedWork(CaseWorker caseWorker)
        {
            WorkerRole role = caseWorker.Role;
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsFinish(role);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionFinish(role);
                    break;

                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.Recordwork: there is no work to do.");
            }
        }



        internal void EnqueueForWork()
        {
            if (_isReadyForOP)
            {
                _opQueue.Enqueue(this);
                Record.SetOPEnqueue();
                return;
            }


            WorkerRole role = Board.EnqueueForNextWorker(this);
            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.SetSummonsEnqueue(role);
                    break;
                case WorkType.Decision:
                    Record.SetDecisionEnqueue(role);
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.EnqueueForWork: Case is not in Summons or Decision stage.");
            
            }
        }

        private bool _isReadyForOP
        { get { return Record.ChairSummons.Finish != null && Record.OP.Enqueue == null; } }
    }
}