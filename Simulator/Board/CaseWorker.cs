using System;

namespace Simulator
{
    internal class CaseWorker
    {
        private BoardQueues _boardQueues = WorkQueues.Members;

        internal readonly Member Member;
        internal readonly WorkerRole Role;


        internal CaseWorker(Member member, WorkerRole role)
        {
            Member = member;
            Role = role;
        }


        internal void Enqueue(AllocatedCase allocatedCase)
        {
            _boardQueues.EnqueueForMember(Member, Role, allocatedCase);
        }        
    }
}