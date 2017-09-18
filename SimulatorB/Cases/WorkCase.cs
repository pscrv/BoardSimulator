using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    internal abstract class WorkCase
    {
        internal abstract CaseWorker Chair { get; }
        internal abstract CaseWorker Rapporteur { get; }
        internal abstract CaseWorker SecondWorker { get; }

        internal abstract bool CurrentWorkerIsFinished { get; }
        internal abstract bool AllWorkersAreFinished { get; }

        internal abstract Member GetNextMember();
        internal abstract void WorkAndPassToRegistrar(Hour currentHour, Registrar registrar);
        internal abstract void ProcessFinishedCase(Hour currentHour, Registrar registrar);
    }



    internal abstract class WorkCaseCommon : WorkCase
    {
        #region abstract
        protected abstract int _getWorkHours(CaseWorker worker);
        protected abstract void _processFinishedCase(Hour currentHour, Registrar registrar);
        protected abstract void _circulate(Registrar registrar);
        #endregion


        #region fields
        protected AppealCase _case;
        protected CaseBoard _workers;

        private Queue<CaseWorker> _workerQueue;
        private Work _work;
        #endregion


        #region construction
        internal WorkCaseCommon(AppealCase appealCase, CaseBoard workers)
        {
            _case = appealCase;
            _workers = workers;
            _workerQueue = new Queue<CaseWorker>();
            foreach (CaseWorker worker in workers)
            {
                _workerQueue.Enqueue(worker);
            }

            if (_workerQueue == null || _workerQueue.Count < 1)
                throw new ArgumentException("Invalid: workers is null or empty.");

            _work = new Work(_getWorkHours(_workerQueue.Peek()));
        }
        #endregion


        #region internal properties and methods
        internal override CaseWorker Chair => _workers.Chair;
        internal override CaseWorker Rapporteur => _workers.Rapporteur;
        internal override CaseWorker SecondWorker => _workers.SecondWorker;


        internal override bool CurrentWorkerIsFinished => _work.IsFinished;
        internal override bool AllWorkersAreFinished => _workerQueue.Count < 1;
        internal override Member GetNextMember() => _workerQueue.FirstOrDefault()?.Member;

        internal override void WorkAndPassToRegistrar(Hour currentHour, Registrar registrar)
        {
            if (_workerQueue.Count < 1)
                throw new InvalidOperationException("No worker queued to do work.");
            
            if (! _work.IsFinished)
                _work.DoWork();

            if (_work.IsFinished)
            {
                _workerQueue.Dequeue();

                if (AllWorkersAreFinished)
                {
                    _processFinishedCase(currentHour, registrar);
                }
                else
                {
                    _circulate(registrar);
                    _work = new Work(_getWorkHoursForNextMember());
                }
            }
        }

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        { 
            _processFinishedCase(currentHour, registrar);
        }
        #endregion


        private int _getWorkHoursForNextMember()
        {
            return _getWorkHours(_workerQueue.Peek());
        }

    }



    internal class SummonsCase : WorkCaseCommon
    {

        internal SummonsCase(AppealCase appealCase, CaseBoard workers)
            : base (appealCase, workers)
        { }


        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursForSummons;
        }

        protected override void _processFinishedCase(Hour currentHour, Registrar registrar)
        {
            registrar.ScheduleOP(currentHour, _case, _workers);
        }

        protected override void _circulate(Registrar registrar)
        {
            registrar.AddToSummonsCirculation(this);
        }
    }


    internal class DecisionCase : WorkCaseCommon
    {
        public DecisionCase(AppealCase appealCase, CaseBoard workers) 
            : base(appealCase, workers)
        { }

        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursForDecision;
        }

        protected override void _processFinishedCase(Hour currentHour, Registrar registrar)
        {
            registrar.AddToFinishedCaseList(_case);
        }

        protected override void _circulate(Registrar registrar)
        {
            registrar.AddToDecisionCirculation(this);
        }
    }



    internal class OPCase : WorkCaseCommon
    {
        public OPCase(AppealCase appealCase, CaseBoard workers) 
            : base(appealCase, workers)
        { }

        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursOPPreparation;
        }

        protected override void _circulate(Registrar registrar)
        {
            throw new NotImplementedException();
        }

        protected override void _processFinishedCase(Hour currentHour, Registrar registrar)
        {
            WorkCase decisionCase = new DecisionCase(_case, _workers);
            registrar.AddToDecisionCirculation(decisionCase);

        }
    }

}
