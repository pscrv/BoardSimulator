using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class CaseQueue
    {
        #region private fields
        private Queue<AppealCase> _queue;
        private Dictionary<AppealCase, Hour> _timeOfEnqueuing;
        #endregion

        #region internal properties
        internal int Count { get { return _queue.Count; } }
        #endregion

        #region consctructors
        internal CaseQueue() 
        {
            _queue = new Queue<AppealCase>();
            _timeOfEnqueuing = new Dictionary<AppealCase, Hour>();
        }
        #endregion


        #region internal methods
        internal void Enqueue(AppealCase ac)
        {
            _queue.Enqueue(ac);
            _timeOfEnqueuing[ac] = SimulationTime.Current;
        }

        internal AppealCase Dequeue()
        {
            if (Count == 0)
                return null;

            AppealCase ac = _queue.Dequeue();
            _timeOfEnqueuing.Remove(ac);
            return ac;
        }
        #endregion
    }



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
        internal void Enqueue(AppealCase appealCase)
        {
            if (appealCase.Stage == AppealCaseState.Stage.New)
                appealCase.AdvanceState();

            switch (appealCase.Stage)
            {
                case AppealCaseState.Stage.SummonsEnqueued:
                case AppealCaseState.Stage.SummonsStarted:
                case AppealCaseState.Stage.SummonsFinished:
                    _summonsQueue.Enqueue(appealCase);
                    break;
                case AppealCaseState.Stage.DecisionEnqueued:
                case AppealCaseState.Stage.DecisionStarted:
                case AppealCaseState.Stage.DecisionFinished:
                    _decisionQueue.Enqueue(appealCase);
                    break;

                default:
                    throw new InvalidOperationException("Appeal is not in Summons or Decision stage.");
            }
        }


        internal AppealCase Dequeue()
        {
            if (_decisionQueue.Count > 0)
                return _decisionQueue.Dequeue();

            return _summonsQueue.Dequeue();
        }
        #endregion

    }

}