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


        private IEnumerable<WorkerRole> Roles
        {
           get
            {
                yield return WorkerRole.Rapporteur;
                yield return WorkerRole.OtherMember;
                yield return WorkerRole.Chair;
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
            foreach (WorkerRole role in Roles)
            {
                _summonsRecords[role] = new ActionRecord(role);
                _decisionRecords[role] = new ActionRecord(role);
            }

        }
        #endregion


        internal void SetAllocation()
        {
            if (Allocation != null)
                throw new InvalidOperationException("Allocation can only be set once.");
            
            Allocation = SimulationTime.Current;
        }
       

        internal void SetSummonsEnqueue(WorkerRole role)
        {
            _summonsRecords[role].SetEnqueue();
        }

        internal void SetSummonsStart(WorkerRole role)
        {
            _summonsRecords[role].SetStart();
        }

        internal void SetSummonsFinish(WorkerRole role)
        {
            _summonsRecords[role].SetFinish();
        }

        internal void SetOPEnqueue()
        {
            OP.SetEnqueue();
        }

        internal void SetOPStart()
        {
            OP.SetStart();
        }

        internal void SetOPFinished()
        {
            OP.SetFinish();
        }

        internal void SetDecisionEnqueue(WorkerRole role)
        {
            _decisionRecords[role].SetEnqueue();
        }

        internal void SetDecisionStart(WorkerRole role)
        {
            _decisionRecords[role].SetStart();
        }

        internal void SetDecisionFinish(WorkerRole role)
        {
            _decisionRecords[role].SetFinish();
        }
    }
}