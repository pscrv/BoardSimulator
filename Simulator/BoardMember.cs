using System;

namespace Simulator
{
    internal abstract class BoardMember
    {
        protected WorkParameters _chairWorkParameters;
        protected SummonsQueue _chairSummonsQueue;
        protected DecisionQueue _chairDecisionQueue;
        protected Work _currentWork;

        protected abstract Work _getNonChairWork();

        protected BoardMember(WorkParameters parameters)
        {
            _chairWorkParameters = parameters;
            _chairSummonsQueue = new SummonsQueue();
            _chairDecisionQueue = new DecisionQueue();
            _currentWork = new Nullwork();
        }

        internal void EnqueueChairWork(SummonsWork work, Hour hour)
        {
            work.SetCounter(_chairWorkParameters.HoursForsummons);
            _chairSummonsQueue.Enqueue(work, hour);
        }

        internal void EnqueueChairWork(DecisionWork work, Hour hour)
        {
            work.SetCounter(_chairWorkParameters.HoursForDecision);
            _chairDecisionQueue.Enqueue(work, hour);
        }

        internal HourlyMemberLog DoWork(Hour hour)
        {
            if (_hasOP(hour))
                return new OPLog(this, hour, new OPWork());

            if (_currentWork.IsFinished)
                _currentWork = _getFromQueue();
            _currentWork.DoWork(hour);
            

            return new HourlyMemberLog(this, hour, _currentWork);

        }

        private Work _getFromQueue()
        {
            if (_chairDecisionQueue.IsNotEmpty)
                return _chairDecisionQueue.Dequeue();
            if (_chairSummonsQueue.IsNotEmpty)
                return _chairSummonsQueue.Dequeue();
            return _getNonChairWork();
        }

        protected bool _hasOP(Hour hour)
        {
            return false;
        }
    }



    internal class Chair : BoardMember
    {
        public Chair(WorkParameters parameters) 
            : base(parameters) { }

        protected override Work _getNonChairWork()
        {
            return null;
        }
    }



    internal class TechnicalMember : BoardMember
    {
        private WorkParameters _rapporteurParameters;
        private SummonsQueue _rapporteurSummonsQueue;
        private DecisionQueue _rapporteurDecisionQueue;

        internal TechnicalMember(WorkParameters chairparameters, WorkParameters rapporteurparameters) 
            : base(chairparameters)
        {
            _rapporteurParameters = rapporteurparameters;
            _rapporteurSummonsQueue = new SummonsQueue();
            _rapporteurDecisionQueue = new DecisionQueue();
        }


        internal void EnqueueRapporteurWork(SummonsWork work, Hour hour)
        {
            work.SetCounter(_rapporteurParameters.HoursForsummons);
            _rapporteurSummonsQueue.Enqueue(work, hour);
        }

        internal void EnqueueRapporteurWork(DecisionWork work, Hour hour)
        {
            work.SetCounter(_rapporteurParameters.HoursForDecision);
            _rapporteurDecisionQueue.Enqueue(work, hour);
        }


        protected override Work _getNonChairWork()
        {
            if (_rapporteurDecisionQueue.IsNotEmpty)
                return _rapporteurDecisionQueue.Dequeue();
            if (_rapporteurSummonsQueue.IsNotEmpty)
                return _rapporteurSummonsQueue.Dequeue();
            return new Nullwork();
        }
    }


    internal class LegalMember : BoardMember
    {
        private WorkParameters _legalParameters;
        private SummonsQueue _legalSummonsQueue;
        private DecisionQueue _legalDecisionQueue;

        internal LegalMember(WorkParameters chairparameters, WorkParameters legalparameters) 
            : base(chairparameters)
        {
            _legalParameters = legalparameters;
            _legalSummonsQueue = new SummonsQueue();
            _legalDecisionQueue = new DecisionQueue();
        }


        internal void EnqueueLegalWork(SummonsWork work, Hour hour)
        {
            work.SetCounter(_legalParameters.HoursForsummons);
            _legalSummonsQueue.Enqueue(work, hour);
        }

        internal void EnqueueLegalWork(DecisionWork work, Hour hour)
        {
            work.SetCounter(_legalParameters.HoursForDecision);
            _legalDecisionQueue.Enqueue(work, hour);
        }

        protected override Work _getNonChairWork()
        {
            if (_legalDecisionQueue.IsNotEmpty)
                return _legalDecisionQueue.Dequeue();
            if (_legalSummonsQueue.IsNotEmpty)
                return _legalSummonsQueue.Dequeue();
            return new Nullwork();
        }
    }
    
}