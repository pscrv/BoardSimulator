using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class CaseRecord
    {
        #region fields and properties
        private Dictionary<WorkerRole, ActionRecord> _summonsRecords;
        private Dictionary<WorkerRole, ActionRecord> _decisionRecords;
        
        internal Hour Creation { get; private set; }
        internal Hour Allocation { get; private set; }
        internal ActionRecord OP { get; private set; }


        internal ActionRecord RapporteurSummons { get { return _summonsRecords[WorkerRole.Rapporteur]; } }
        internal ActionRecord OtherMemberSummons { get { return _summonsRecords[WorkerRole.OtherMember]; } }
        internal ActionRecord ChairSummons { get { return _summonsRecords[WorkerRole.Chair]; } }
        
        internal ActionRecord RapporteurDecision { get { return _decisionRecords[WorkerRole.Rapporteur]; } }
        internal ActionRecord OtherMemberDecision { get { return _decisionRecords[WorkerRole.OtherMember]; } }
        internal ActionRecord ChairDecision { get { return _decisionRecords[WorkerRole.Chair]; } }


        private IEnumerable<WorkerRole> _roles
        {
           get
            {
                yield return WorkerRole.Rapporteur;
                yield return WorkerRole.OtherMember;
                yield return WorkerRole.Chair;
            }
        }


        internal CaseStage Stage
        {
            get
            {
                if (RapporteurSummons.Finish == null
                    || OtherMemberSummons.Finish == null
                    || ChairSummons.Finish == null)
                    return CaseStage.Summons;

                if (OP.Finish == null)
                    return CaseStage.OP;

                if (RapporteurDecision.Finish == null
                    || OtherMemberDecision.Finish == null
                    || ChairDecision.Finish == null)
                    return CaseStage.Decision;

                return CaseStage.Finished;
            }
        }

        #endregion


        #region construction
        internal CaseRecord(AppealCase ac)
        {
            Creation = ac.CreationHour;
            OP = new ActionRecord(WorkerRole.None);

            _summonsRecords = new Dictionary<WorkerRole, ActionRecord>();
            _decisionRecords = new Dictionary<WorkerRole, ActionRecord>();
            foreach (WorkerRole role in _roles)
            {
                _summonsRecords[role] = new ActionRecord(role);
                _decisionRecords[role] = new ActionRecord(role);
            }

        }
        #endregion


        #region records
        //working
        //  used only in AllocatedCase.EnqueueForWork
        internal void RecordEnqueuedForWork(WorkerRole role, Hour currentHour)
        {
            switch (Stage)
            {
                case CaseStage.Summons:
                    SetSummonsEnqueue(role, currentHour);
                    break;
                case CaseStage.Decision:
                    SetDecisionEnqueue(role, currentHour);
                    break;
                case CaseStage.OP:
                    SetOPEnqueue(currentHour);
                    break;

                default:
                    throw new InvalidOperationException("CaseRecord.EnqueueForWork: Case is not in Summons or Decision stage.");

            }            
        }

        //working
        // used only in 
        //  AllocatedCase.RecordStartOfWork (unused)
        //  AllocatedCase._setNewWorker (used)
        internal void RecordStartOfWork(WorkerRole role, Hour currentHour)
        {
            switch (Stage)
            {
                case CaseStage.Summons:
                    SetSummonsStart(role, currentHour);
                    break;
                case CaseStage.Decision:
                    SetDecisionStart(role, currentHour);
                    break;
                case CaseStage.OP:
                    SetOPStart(currentHour);
                    break;

                default:
                    throw new InvalidOperationException("CaseRecord.RecordStartOfWork: no summons or decision work to start.");
            }
        }

        //working
        // used only in 
        //  AllocatedCase.DoWorkAndMakeReport
        //  RecordFinishedWork (unsed)
        internal void RecordFinishedWork(WorkerRole role, Hour currentHour)
        {
            switch (Stage)
            {
                case CaseStage.Summons:
                    SetSummonsFinish(role, currentHour);
                    break;
                case CaseStage.Decision:
                    SetDecisionFinish(role, currentHour);
                    break;
                case CaseStage.OP:
                    SetOPFinished(currentHour);
                    break;

                default:
                    throw new InvalidOperationException("CaseRecord.Recordwork: there is no work to do.");
            }
        }
        #endregion


        #region Record setters
        internal void SetAllocation(Hour currentHour)  //ok
        {
            if (Allocation != null)
                throw new InvalidOperationException("Allocation can only be set once.");
            
            Allocation = currentHour;
        }       

        internal void SetSummonsEnqueue(WorkerRole role, Hour currentHour) //ok
        {
            _summonsRecords[role].SetEnqueue(currentHour);
        }

        internal void SetSummonsStart(WorkerRole role, Hour currentHour) //ok
        {
            _summonsRecords[role].SetStart(currentHour);
        }

        internal void SetSummonsFinish(WorkerRole role, Hour currentHour) //ok
        {
            _summonsRecords[role].SetFinish(currentHour);
        }

        internal void SetOPEnqueue(Hour currentHour) //ok
        {
            OP.SetEnqueue(currentHour);
        }

        internal void SetOPStart(Hour currentHour) //ok
        {
            OP.SetStart(currentHour);
        }

        internal void SetOPFinished(Hour currentHour) //ok
        {
            OP.SetFinish(currentHour);
        }

        internal void SetDecisionEnqueue(WorkerRole role, Hour currentHour) //ok
        {
            _decisionRecords[role].SetEnqueue(currentHour);
        }

        internal void SetDecisionStart(WorkerRole role, Hour currentHour) //ok
        {
            _decisionRecords[role].SetStart(currentHour);
        }

        internal void SetDecisionFinish(WorkerRole role, Hour currentHour) //ok
        {
            _decisionRecords[role].SetFinish(currentHour);
        }
        #endregion
        
    }
}