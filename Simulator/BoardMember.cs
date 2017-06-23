using System;

namespace Simulator
{
    internal abstract class BoardWorker { }


    internal abstract class ChairWorker : BoardWorker
    {
        protected WorkParameters _chairWorkParameters;
        protected SummonsQueue _chairSummonsQueue;
        protected DecisionQueue _chairDecisionQueue;
        protected Work _currentWork;

        protected abstract Work _getNonChairWork();

        protected ChairWorker(WorkParameters parameters)
        {
            _chairWorkParameters = parameters;
            _chairSummonsQueue = new SummonsQueue();
            _chairDecisionQueue = new DecisionQueue();
            _currentWork = new NullWork();
        }

        internal void EnqueueChairWork(SummonsCase sc, Hour hour)
        {
            SummonsWork work = new SummonsWork(sc, _chairWorkParameters.HoursForsummons);
            _chairSummonsQueue.Enqueue(work, hour);
        }

        internal void EnqueueChairWork(DecisionCase dc, Hour hour)
        {
            DecisionWork work = new DecisionWork(dc, _chairWorkParameters.HoursForDecision);
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


    internal abstract class NonChairWorker : ChairWorker
    {
        private WorkParameters _nonChairParameters;
        private SummonsQueue _nonChairsummonsQueue;
        private DecisionQueue _nonChairDecisionQueue;

        internal NonChairWorker(WorkParameters chairparameters, WorkParameters nonchairparameters) 
            : base(chairparameters)
        {
            _nonChairParameters = nonchairparameters;
            _nonChairsummonsQueue = new SummonsQueue();
            _nonChairDecisionQueue = new DecisionQueue();
        }


        protected void _enqueueNonChairWork(SummonsCase sc, Hour hour)
        {
            SummonsWork work = new SummonsWork(sc, _nonChairParameters.HoursForsummons);
            _nonChairsummonsQueue.Enqueue(work, hour);
        }

        protected void _enqueueNonChairWork(DecisionCase dc, Hour hour)
        {
            DecisionWork work = new DecisionWork(dc, _nonChairParameters.HoursForDecision);
            _nonChairDecisionQueue.Enqueue(work, hour);
        }


        protected override Work _getNonChairWork()
        {
            if (_nonChairDecisionQueue.IsNotEmpty)
                return _nonChairDecisionQueue.Dequeue();
            if (_nonChairsummonsQueue.IsNotEmpty)
                return _nonChairsummonsQueue.Dequeue();
            return new NullWork();
        }
    }





    internal class Chair : ChairWorker
    {
        public Chair(WorkParameters parameters) 
            : base(parameters) { }

        protected override Work _getNonChairWork()
        {
            return new NullWork();
        }
    }


    internal class TechnicalMember : NonChairWorker
    {

        internal TechnicalMember(WorkParameters chairparameters, WorkParameters rapporteurparameters)
            : base(chairparameters, rapporteurparameters) { }


        internal void EnqueueRapporteurWork(SummonsCase sc, Hour hour)
        {
            base._enqueueNonChairWork(sc, hour);
        }

        internal void EnqueueRapporteurWork(DecisionCase dc, Hour hour)
        {
            base._enqueueNonChairWork(dc, hour);
        }
    }


    internal class LegalMember : NonChairWorker
    {
        internal LegalMember(WorkParameters chairparameters, WorkParameters legalparameters) 
            : base(chairparameters, legalparameters) { }


        internal void EnqueueLegalWork(SummonsCase sc, Hour hour)
        {
            base._enqueueNonChairWork(sc, hour);
        }

        internal void EnqueueLegalWork(DecisionCase dc, Hour hour)
        {
            base._enqueueNonChairWork(dc, hour);
        }
    }
    
}