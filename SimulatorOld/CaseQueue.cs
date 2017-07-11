using System;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class CaseQueue<T>
    {
        #region private fields
        private Queue<T> _queue;
        private Dictionary<T, Hour> _timeOfEnqueuing;
        #endregion

        #region internal properties
        internal int Count { get { return _queue.Count; } }
        #endregion

        #region consctructors
        internal CaseQueue() 
        {
            _queue = new Queue<T>();
            _timeOfEnqueuing = new Dictionary<T, Hour>();
        }
        #endregion


        #region internal methods
        internal void Enqueue(T t)
        {
            _queue.Enqueue(t);
            _timeOfEnqueuing[t] = SimulationTime.Current;
        }

        internal T Dequeue()
        {
            if (Count == 0)
                return default(T);

            T t = _queue.Dequeue();
            _timeOfEnqueuing.Remove(t);
            return t;
        }
        #endregion
    }


    internal class AppealCaseQueue : CaseQueue<AppealCase> { }



    internal class CaseQueuePair
    {
        #region private fields
        private AppealCaseQueue _summonsQueue;
        private AppealCaseQueue _decisionQueue;
        #endregion


        #region internal properties
        internal int Count { get { return _decisionQueue.Count + _summonsQueue.Count; } }
        #endregion


        #region constructors
        internal CaseQueuePair()
        {
            _summonsQueue = new AppealCaseQueue();
            _decisionQueue = new AppealCaseQueue();
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