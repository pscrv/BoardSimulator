using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class WorkerQueue
    {
        #region private fields
        private Queue<Member> _summonsQueue;
        private Queue<Member> _decisionQueue;
        #endregion


        #region internal properties
        internal int SummonsWorkerCount { get { return _summonsQueue.Count; } }
        internal int DecisionWorkerCount { get { return _decisionQueue.Count; } }
        #endregion


        #region constructors
        internal WorkerQueue(Member chair, Member rapporteur, Member other)
        {
            _summonsQueue = new Queue<Member>();
            _decisionQueue = new Queue<Member>();
            _enqueueMembers(_summonsQueue, chair, rapporteur, other);
            _enqueueMembers(_decisionQueue, chair, rapporteur, other);
        }

        #endregion


        #region internal methods
        //internal void Enqueue(Member member)
        //{
        //    _summonsQueue.Enqueue(member);
        //}

        internal Member DequeueSummonsWorker()
        {
            return _summonsQueue.Dequeue();
        }

        internal Member DequeueDecisionWorker()
        {
            return _decisionQueue.Dequeue();
        }
        #endregion


        #region private methods
        private void _enqueueMembers(Queue<Member> queue, Member chair, Member rapporteur, Member other)
        {
            queue.Enqueue(rapporteur);
            queue.Enqueue(other);
            queue.Enqueue(chair);
        }
        #endregion
    }
}