using System;

namespace Simulator
{
    internal class CaseQueuePair
    {
        #region private fields
        private CaseQueue _summonsQueue;
        private CaseQueue _decisionQueue;
        #endregion


        #region internal properties
        internal int Count { get { return _decisionQueue.Count + _summonsQueue.Count; } }
        #endregion


        #region constructors
        internal CaseQueuePair()
        {
            _summonsQueue = new CaseQueue();
            _decisionQueue = new CaseQueue();
        }
        #endregion


        #region internal methods
        internal void Enqueue(Hour currentHour, AllocatedCase ac)
        {
            switch (ac.Stage)
            {
                case CaseStage.Summons:
                    _summonsQueue.Enqueue(currentHour, ac);
                    break;
                case CaseStage.Decision:
                    _decisionQueue.Enqueue(currentHour, ac);
                    break;
                case CaseStage.OP:
                case CaseStage.Finished:
                    throw new InvalidOperationException("Appeal is not in Summons or Decision stage.");
            }
        }

        internal AllocatedCase Dequeue()
        {
            if (_decisionQueue.Count > 0)
                return _decisionQueue.Dequeue();

            return _summonsQueue.Dequeue();
        }


        internal AllocatedCase Peek()
        {
            if (_decisionQueue.Count > 0)
                return _decisionQueue.Peek();

            return _summonsQueue.Peek();
        }
        #endregion

    }
}