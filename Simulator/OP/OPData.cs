using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class OPData
    {
        private Dictionary<WorkerRole, bool> _isMemberWorking;
        internal AllocatedCase AllocatedCase { get; private set; }   
        

        internal OPData(AllocatedCase ac, WorkerRole role)
        {
            AllocatedCase = ac;
            _isMemberWorking = new Dictionary<WorkerRole, bool>
            {
                [WorkerRole.Chair] = false,
                [WorkerRole.Rapporteur] = false,
                [WorkerRole.OtherMember] = false
            };
            _isMemberWorking[role] = true;
        }

        internal bool IsRoleWorking(WorkerRole role)
        {
            return _isMemberWorking[role];
        }

        internal void MarkAsActive(WorkerRole role)
        {
            _isMemberWorking[role] = true;
        }
    }
}
