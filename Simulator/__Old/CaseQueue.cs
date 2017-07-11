using System;
using System.Collections.Generic;

namespace OldSim
{
    internal class CaseQueue<T>
    {
        #region private fields
        private Queue<T> _queue;
        private Dictionary<T, Hour> _timeOfEnqueuing;
        #endregion

        #region internal properties
        internal int Count { get { return _queue.Count; } }
        internal int Age { get { return SimulationTime.Current.Value - _timeOfEnqueuing[_queue.Peek()].Value; } }
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

    internal class AllocatedCaseQueue : CaseQueue<AllocatedCase> { }



    internal class AppealCaseQueuePair
    {
        #region private fields
        private AppealCaseQueue _summonsQueue;
        private AppealCaseQueue _decisionQueue;
        #endregion


        #region internal properties
        internal int Count { get { return _decisionQueue.Count + _summonsQueue.Count; } }
        internal int Age { get { return Math.Max(_decisionQueue.Age, _summonsQueue.Age); } }
        #endregion


        #region constructors
        internal AppealCaseQueuePair()
        {
            _summonsQueue = new AppealCaseQueue();
            _decisionQueue = new AppealCaseQueue();
        }
        #endregion


        #region internal methods
        internal void Enqueue(AppealCase appealCase)
        {
            switch (appealCase.Stage)
            {
                case AppealCaseState.Stage.New:
                    _summonsQueue.Enqueue(appealCase);
                    appealCase.AdvanceState();
                    break;
                case AppealCaseState.Stage.OPFinsished:
                    _decisionQueue.Enqueue(appealCase);
                    appealCase.AdvanceState();
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


    internal class AllocatedCaseQueuePair
    {
        #region private fields
        private AllocatedCaseQueue _summonsQueue;
        private AllocatedCaseQueue _decisionQueue;
        #endregion


        #region internal properties
        internal int Count { get { return _decisionQueue.Count + _summonsQueue.Count; } }
        internal int Age { get { return Math.Max(_decisionQueue.Age, _summonsQueue.Age); } }
        #endregion


        #region constructors
        internal AllocatedCaseQueuePair()
        {
            _summonsQueue = new AllocatedCaseQueue();
            _decisionQueue = new AllocatedCaseQueue();
        }
        #endregion


        #region internal methods
        internal void Enqueue(AllocatedCase aCase)
        {
            if (aCase.IsInSummonsStage)
            {
                _summonsQueue.Enqueue(aCase);
            }
            else if (aCase.IsInDecisionStage)
            {
                _decisionQueue.Enqueue(aCase);
            }
            else
            {
                throw new InvalidOperationException("Appeal is not in Summons or Decision stage.");
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