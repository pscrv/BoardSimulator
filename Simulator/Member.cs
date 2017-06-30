using System;

namespace Simulator
{
    internal class Member
    {
        #region TEMPORARY AREA  --- STATIC
        private static int HOURSPERSUMMONS = 2;
        private static int HOURSPERDECISION = 3;
        #endregion


        #region private fields
        private CaseQueuePair _rapporteurQueue;
        private CaseQueuePair _chairQueue;
        private CaseQueuePair _otherQueue;

        private AppealCase _currentCase;
        private int _workCounter;
        #endregion


        #region internal properties
        internal int CaseCount { get { return _rapporteurQueue.Count + _chairQueue.Count + _otherQueue.Count; } }
        #endregion




        #region consctructor
        internal Member()
        {
            _rapporteurQueue = new CaseQueuePair();
            _chairQueue = new CaseQueuePair();
            _otherQueue = new CaseQueuePair();

            _currentCase = null;
            _workCounter = 0;
        }
        #endregion

        #region internal methods
        internal void EnqueueRapporteurWork(AppealCase appealCase)
        {
            _rapporteurQueue.Enqueue(appealCase);
        }

        internal void EnqueueChairWork(AppealCase appealCase)
        {
            _chairQueue.Enqueue(appealCase);
        }

        internal void EnqueueOtherWork(AppealCase appealCase)
        {
            _otherQueue.Enqueue(appealCase);
        }

        internal void DoWork()
        {
            if (_currentCase == null)
            {
                _currentCase = _getNextCase();
                if (_currentCase == null)
                    return;

                switch (_currentCase.Stage)
                {
                    case AppealCaseState.Stage.SummonsEnqueued:
                        _currentCase.AdvanceState();
                        _workCounter = HOURSPERSUMMONS;
                        break;
                    case AppealCaseState.Stage.DecisionEnqueued:
                        _currentCase.AdvanceState();
                        _workCounter = HOURSPERDECISION;
                        break;
                    default:
                        throw new InvalidOperationException("Newly dequeued _currentCase is in an invalid state.");
                }

                switch (_currentCase.Stage)
                {
                    case AppealCaseState.Stage.SummonsStarted:
                    case AppealCaseState.Stage.DecisionStarted:
                        _workCounter--;
                        break;
                    default:
                        throw new InvalidOperationException("_currentCase is in an invalid state.");
                }

                if (_workCounter == 0)
                {
                    _currentCase.AdvanceState();
                    _currentCase = null;
                }
            }

        }
        #endregion


        #region private methods
        private AppealCase _getNextCase()
        {
            if (_chairQueue.Count > 0)
                return _chairQueue.Dequeue();
            if (_otherQueue.Count > 0)
                return _otherQueue.Dequeue();
            if (_rapporteurQueue.Count > 0)
                return _rapporteurQueue.Dequeue();
            return null;
        }

        #endregion
    }
}