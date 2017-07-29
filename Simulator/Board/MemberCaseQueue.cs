using System;

namespace Simulator
{
    internal class MemberCaseQuue
    {
        #region private fields
        private CaseQueue _summonsQueue;
        private CaseQueue _decisionQueue;
        #endregion


        #region internal properties
        internal int Count { get { return _decisionQueue.Count + _summonsQueue.Count; } }
        #endregion


        #region constructors
        internal MemberCaseQuue()
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
                    throw new InvalidOperationException("Can only enqueue for member during summons or decision stage.");
            }
        }


        internal AllocatedCase Dequeue()
        {
            if (_decisionQueue.Count > 0)
                return _decisionQueue.Dequeue();

            return _summonsQueue.Dequeue();
        }
        #endregion

    }
}