using System;

namespace Simulator
{
    internal class BoardWorker
    {
        #region private fields
        private WorkParameters _chairParameters;
        private WorkParameters _rapporteurParameters;
        private WorkParameters _otherParameters;

        private Work _currentWork;
        private WorkType _currentWorkType;
        private SummonsQueue _chairSummonsQueue;
        private SummonsQueue _rapporteurSummonsQueue;
        private SummonsQueue _otherSummonsQueue;
        private DecisionQueue _chairDecisionQueue;
        private DecisionQueue _rapporteurDecisionQueue;
        private DecisionQueue _otherDecisionQueue;
        #endregion

        internal int TotalWorkCount
        {
            get
            {
                return
                    _chairDecisionQueue.Count +
                    _chairSummonsQueue.Count +
                    _rapporteurDecisionQueue.Count +
                    _rapporteurSummonsQueue.Count +
                    _otherDecisionQueue.Count +
                    _otherSummonsQueue.Count;                
            }
        }


        #region constructors
        internal BoardWorker(WorkParameters chairparameters, WorkParameters rapporteurparameters, WorkParameters otherparameters)
        {
            _chairParameters = chairparameters;
            _rapporteurParameters = rapporteurparameters;
            _otherParameters = otherparameters;

            _currentWork = new NullWork();
            _currentWorkType = WorkType.NoWork;
            _chairDecisionQueue = new DecisionQueue();
            _chairSummonsQueue = new SummonsQueue();
            _rapporteurDecisionQueue = new DecisionQueue();
            _rapporteurSummonsQueue = new SummonsQueue();
            _otherDecisionQueue = new DecisionQueue();
            _otherSummonsQueue = new SummonsQueue();
        }
        #endregion


        #region internal methods
        internal void EnqueueChairWork(DecisionCase dc, Hour h)
        {
            _enqueueDecisionWork(dc, _chairParameters, _chairDecisionQueue, h);
        }

        internal void EnqueueChairWork(SummonsCase sc, Hour h)
        {
            _enqueueSummonsWork(sc, _chairParameters, _chairSummonsQueue, h);
        }

        
        internal void EnqueueRapporteurWork(DecisionCase dc, Hour h)
        {
            _enqueueDecisionWork(dc, _rapporteurParameters, _rapporteurDecisionQueue, h);
        }

        internal void EnqueueRapporteurWork(SummonsCase sc, Hour h)
        {
            _enqueueSummonsWork(sc, _rapporteurParameters, _rapporteurSummonsQueue, h);
        }

        internal void EnqueueOtherWork(DecisionCase dc, Hour h)
        {
            _enqueueDecisionWork(dc, _otherParameters, _otherDecisionQueue, h);
        }

        internal void EnqueueOtherWork(SummonsCase sc, Hour h)
        {
            _enqueueSummonsWork(sc, _otherParameters, _otherSummonsQueue, h);
        }



        internal HourlyworkerLog DoWork()
        {
            if (_currentWork.IsFinished)
                _setCurrentWorkFromQueue();
            _currentWork.DoWork();

            return new HourlyworkerLog(_currentWorkType, _currentWork.Case);
        }
        #endregion

        #region private methods
        private void _setCurrentWorkFromQueue()
         {
            if (_chairDecisionQueue.IsNotEmpty)
            {
                _currentWork = _chairDecisionQueue.Dequeue();
                _currentWorkType = WorkType.DecisionWork;
                return;
            }
            if (_chairSummonsQueue.IsNotEmpty)
            {
                _currentWork = _chairSummonsQueue.Dequeue();
                _currentWorkType = WorkType.SummonsWork;
                return;
            }
            if (_otherDecisionQueue.IsNotEmpty)
            {
                _currentWork = _otherDecisionQueue.Dequeue();
                _currentWorkType = WorkType.DecisionWork;
                return;
            }
            if (_otherSummonsQueue.IsNotEmpty)
            {
                _currentWork = _otherSummonsQueue.Dequeue();
                _currentWorkType = WorkType.SummonsWork;
                return;
            }
            if (_rapporteurDecisionQueue.IsNotEmpty)
            {
                _currentWork = _rapporteurDecisionQueue.Dequeue();
                _currentWorkType = WorkType.DecisionWork;
                return;
            }
            if (_rapporteurSummonsQueue.IsNotEmpty)
            {
                _currentWork = _rapporteurSummonsQueue.Dequeue();
                _currentWorkType = WorkType.SummonsWork;
                return;
            }

            _currentWork = new NullWork();
            _currentWorkType = WorkType.NoWork;
        }


        private void _enqueueDecisionWork(DecisionCase dc, WorkParameters parameters, DecisionQueue queue, Hour h)
        {
            DecisionWork work = new DecisionWork(dc, parameters.HoursForDecision);
            queue.Enqueue(work, h);
        }

        private void _enqueueSummonsWork(SummonsCase sc, WorkParameters parameters, SummonsQueue queue, Hour h)
        {
            SummonsWork work = new SummonsWork(sc, parameters.HoursForsummons);
            queue.Enqueue(work, h);
        }


        #endregion
    } 
}