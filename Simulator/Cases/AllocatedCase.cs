using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class AllocatedCase
    {
        #region fields and properties
        private Member _currentWorkingMember = null;
        private int _workCounter = 0;
               

        internal readonly AppealCase Case;
        internal readonly CaseBoard Board;
        internal readonly CaseRecord Record;

        //TODO: fix
        internal CaseStage Stage { get => Record.Stage; }
        internal CStage CStage { get; private set; }
        #endregion


        #region construction
        internal AllocatedCase(
            AppealCase ac,
            CaseBoard bd,
            Hour currentHour)
        {
            Case = ac;
            Board = bd;

            Record = new CaseRecord(ac);
            Record.SetAllocation(currentHour);


            CStage = new CStageMachine().Current;
        }
        #endregion



        #region internal methods
        internal WorkReport DoWorkAndMakeReport(Member member, Hour currentHour)
        {
            CaseWorker worker = Board.GetMemberAsCaseWorker(member);
            if (Record.Stage == CaseStage.OP)
            {
                return WorkReport.MakeOPReport(Case, worker.Role);
            }

            //working
            if (CStage.Stage == CaseStage.OP)
            {
                if (Record.OP.Finish != null)
                    CStage = CStage.Next();
            }

            WorkState workState = _doWork(worker, currentHour);
            if (workState == WorkState.Finished)
            {
                Record.RecordFinishedWork(worker.Role, currentHour);

                //working
                if (worker.Role == WorkerRole.Chair)
                    CStage = CStage.Next();
            }

            return WorkReport.MakeReport(
                Case,
                Stage,
                worker.Role,
                workState);
        }


        //unused
        internal void RecordStartOfWork(CaseWorker caseWorker, Hour currentHour)
        {
            Record.RecordStartOfWork(caseWorker.Role, currentHour);
        }

        //unused
        internal void RecordFinishedWork(WorkerRole role, Hour currentHour)
        {
            Record.RecordFinishedWork(role, currentHour);
        }

        //unused
        internal void RecordOPEnqueued(Hour currentHour)
        {
            Record.SetOPEnqueue(currentHour);
        }

        // used in OPSchedule
        internal void RecordOPStart(Hour currentHour)
        {
            Record.SetOPStart(currentHour);
        }

        // used in OPSchedule
        internal void RecordOPFinished(Hour currentHour)
        {
            Record.SetOPFinished(currentHour);
        }

        //unused
        internal WorkerRole GetRole(Member member)
        {
            return Board.GetRole(member);
        }

        // used in OPSchedule
        internal CaseWorker GetCaseWorkerByRole(WorkerRole role)
        {
            return Board.GetCaseWorkerByRole(role);
        }

        // used in OPSchedule
        internal Member GetMemberByRole(WorkerRole role)
        {
            return Board.GetMemberByRole(role);
        }

        // used in Registrar and CaseBuffer
        internal void EnqueueForWork(Hour currentHour)
        {
            WorkerRole role = WorkerRole.None;

            switch (Record.Stage)
            {
                case CaseStage.Undefined:
                case CaseStage.Finished:
                    return;

                case CaseStage.OP:
                    Board.ScheduleOP(currentHour, this);
                    break;

                case CaseStage.Summons:
                case CaseStage.Decision:
                    role = Board.EnqueueForNextWorker(currentHour, this);
                    break;

            }

            Record.RecordEnqueuedForWork(role, currentHour);
        }

        #endregion


        #region private methods

        private WorkState _doWork(CaseWorker worker, Hour currentHour)
        {
            if (worker == null)
                throw new InvalidOperationException("AllocatedCase.Dowork: worker is null.");

            if (_currentWorkingMember == null)
            {
                _setNewWorker(worker, currentHour);
            }

            if (worker.Member != _currentWorkingMember)
                throw new InvalidOperationException("AllocatedCase.Dowork: previous member has not yet finished.");

            _workCounter--;

            if (_workCounter == 0)
            {
                _currentWorkingMember = null;
                return WorkState.Finished;
            }

            return WorkState.Ongoing;
        }

        
        //TODO: consider whether this should be in CaseWorker
        private void _setNewWorker(CaseWorker worker, Hour currentHour)
        {
            _currentWorkingMember = worker.Member;
            _workCounter = worker.HourForStage(Record.Stage);
            Record.RecordStartOfWork(worker.Role, currentHour);
        }

        #endregion



        #region overrides
        public override string ToString()
        {
            return string.Format("Allocated <{0}>", Case.ToString());
        }

        #endregion
    }
}