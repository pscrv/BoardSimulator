using System;

namespace Simulator
{
    internal class CaseWorker
    {
        internal readonly Member Member;
        internal readonly WorkerRole Role;


        internal CaseWorker(Member member, WorkerRole role)
        {
            Member = member;
            Role = role;
        }



        internal void Enqueue(AllocatedCase allocatedCase)
        {
            WorkQueues.EnqueueForMember(Member, Role, allocatedCase);
        }


        
    }
}