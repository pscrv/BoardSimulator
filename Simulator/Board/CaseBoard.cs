using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class CaseBoard
    {
        #region fields and properties
        private Dictionary<WorkType, Queue<CaseWorker>> _queues;

        internal readonly CaseWorker Chair;
        internal readonly CaseWorker Rapporteur;
        internal readonly CaseWorker OtherMember;
        
        internal Queue<CaseWorker> SummonsQueue { get { return _queues[WorkType.Summons]; } }
        internal Queue<CaseWorker> DecisionQueue { get { return _queues[WorkType.Decision]; } }


        internal IEnumerable<CaseWorker> Members
        {
            get
            {
                yield return Rapporteur;
                yield return OtherMember;
                yield return Chair;
            }
        }
        #endregion


        #region construction
        internal CaseBoard(Member ch, Member rp, Member om)
        {
            Chair = new CaseWorker(ch, WorkerRole.Chair);
            Rapporteur = new CaseWorker(rp, WorkerRole.Rapporteur);
            OtherMember = new CaseWorker(om, WorkerRole.OtherMember);

            _queues = new Dictionary<WorkType, Queue<CaseWorker>>();
            _queues[WorkType.Summons] = _makeQueue();
            _queues[WorkType.Decision] = _makeQueue();
        }


        private Queue<CaseWorker> _makeQueue()
        {
            Queue<CaseWorker> queue = new Queue<CaseWorker>();
            queue.Enqueue(Rapporteur);
            queue.Enqueue(OtherMember);
            queue.Enqueue(Chair);
            return queue;
        }
        #endregion





        internal CaseWorker GetMemberAsCaseWorker(Member member)
        {
            if (member == Chair.Member)
                return Chair;
            if (member == Rapporteur.Member)
                return Rapporteur;
            if (member == OtherMember.Member)
                return OtherMember;
            return null;
        }

        internal WorkerRole GetRole(Member member)
        {
            if (member == Chair.Member)
                return WorkerRole.Chair;
            if (member == Rapporteur.Member)
                return WorkerRole.Rapporteur;
            if (member == OtherMember.Member)
                return WorkerRole.OtherMember;
            return WorkerRole.None;
        }

        internal WorkerRole EnqueueForNextWorker(AllocatedCase allocatedCase)
        {
            CaseWorker nextWorker = null;

            if (_queues[WorkType.Summons].Count > 0)
                nextWorker = _queues[WorkType.Summons].Dequeue();
            else if (_queues[WorkType.Decision].Count > 0)
                nextWorker = _queues[WorkType.Decision].Dequeue();

            if (nextWorker == null)
                return WorkerRole.None;


            // TODO : decide which of the following two lines is better
            //nextWorker.Enqueue(allocatedCase);
            nextWorker.Member.CaseQueue.Enqueue(allocatedCase, nextWorker.Role);
            return nextWorker.Role;
        }
        

        internal void EnqueueForOP()
        {
            // TODO: do something here.
        }
    }
}