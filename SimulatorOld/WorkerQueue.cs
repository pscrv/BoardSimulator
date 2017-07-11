using System;
using System.Collections;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class WorkerQueue : IEnumerable<Member>
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

            _summonsQueue.Enqueue(rapporteur);
            _summonsQueue.Enqueue(other);
            _summonsQueue.Enqueue(chair);
        }

        #endregion


        #region internal methods
        internal Member DequeueSummonsWorker()
        {
            Member member = _summonsQueue.Dequeue();
            _decisionQueue.Enqueue(member);
            return member;
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


        #region IEnumerable
        public IEnumerator<Member> GetEnumerator()
        {
            foreach (Member m in _decisionQueue) yield return m;
            foreach (Member m in _summonsQueue) yield return m;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}