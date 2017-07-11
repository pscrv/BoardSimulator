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
        internal int Age { get { return Math.Max(_decisionQueue.Age, _summonsQueue.Age); } }
        #endregion


        #region constructors
        internal MemberCaseQuue()
        {
            _summonsQueue = new CaseQueue();
            _decisionQueue = new CaseQueue();
        }
        #endregion


        #region internal methods
        internal void Enqueue(AllocatedCase ac)
        {
            switch (ac.Stage)
            {
                case CaseStage.Summons:
                    _summonsQueue.Enqueue(ac);
                    break;
                case CaseStage.Decision:
                    _decisionQueue.Enqueue(ac);
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