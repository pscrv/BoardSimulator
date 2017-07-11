using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class MemberWorkQueue
    {
        private Dictionary<WorkerRole, CaseQueuePair> _queues;


        internal int Count
        {
            get
            {
                return
                    _queues[WorkerRole.Rapporteur].Count +
                    _queues[WorkerRole.OtherMember].Count +
                    _queues[WorkerRole.Chair].Count;
            }
        }
        

        internal MemberWorkQueue()
        {
            _queues = new Dictionary<WorkerRole, CaseQueuePair>();
            _queues[WorkerRole.Rapporteur] = new CaseQueuePair();
            _queues[WorkerRole.Chair] = new CaseQueuePair();
            _queues[WorkerRole.OtherMember] = new CaseQueuePair();
        }


        internal void Enqueue(AllocatedCase ac, WorkerRole role)
        {
            _queues[role].Enqueue(ac);
        }


        internal AllocatedCase Dequeue()
        {
            if (_queues[WorkerRole.Chair].Count > 0)
                return _queues[WorkerRole.Chair].Dequeue();
            if (_queues[WorkerRole.OtherMember].Count > 0)
                return _queues[WorkerRole.OtherMember].Dequeue();
            if (_queues[WorkerRole.Rapporteur].Count > 0)
                return _queues[WorkerRole.Rapporteur].Dequeue();
            return null;
        }

        internal AllocatedCase Peek()
        {
            if (_queues[WorkerRole.Chair].Count > 0)
                return _queues[WorkerRole.Chair].Peek();
            if (_queues[WorkerRole.OtherMember].Count > 0)
                return _queues[WorkerRole.OtherMember].Peek();
            if (_queues[WorkerRole.Rapporteur].Count > 0)
                return _queues[WorkerRole.Rapporteur].Peek();
            return null;
        }


        public override string ToString()
        {
            return string.Format("MemberWorkQueue {0}");
        }

    }
}