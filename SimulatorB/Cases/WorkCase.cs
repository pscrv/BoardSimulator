using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    internal abstract class WorkCase
    {
        #region abstract
        protected abstract int _getWorkHours(CaseWorker worker);

        internal abstract void ProcessFinishedCase(Hour currentHour, Registrar registrar);
        internal abstract void Circulate(Registrar registrar);
        #endregion


        #region fields
        protected AppealCase _case;
        protected CaseBoard _workers;
        private Queue<CaseWorker> _workerQueue;
        private Work _work;
        #endregion


        #region construction
        internal WorkCase(AppealCase appealCase, CaseBoard workers)
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
        internal CaseWorker Chair => _workers.Chair;
        internal CaseWorker Rapporteur => _workers.Rapporteur;
        internal CaseWorker SecondWorker => _workers.SecondWorker;


        internal bool CurrentWorkerIsFinished => _work.IsFinished;
        internal bool AllWorkersAreFinished => _workerQueue.Count < 1;
        internal Member GetNextMember() => _workerQueue.FirstOrDefault()?.Member;

        internal void WorkAndPassToRegistrar(Hour currentHour, Registrar registrar)
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
                    ProcessFinishedCase(currentHour, registrar);
                }
                else
                {
                    Circulate(registrar);
                    _work = new Work(_getWorkHoursForNextMember());
                }
            }
        }
        #endregion


        private int _getWorkHoursForNextMember()
        {
            return _getWorkHours(_workerQueue.Peek());
        }

    }



    internal class SummonsCase : WorkCase
    {

        internal SummonsCase(AppealCase appealCase, CaseBoard workers)
            : base (appealCase, workers)
        { }


        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursForSummons;
        }

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            registrar.ScheduleOP(currentHour, _case, _workers);
        }

        internal override void Circulate(Registrar registrar)
        {
            registrar.AddToSummonsCirculation(this);
        }
    }


    internal class DecisionCase : WorkCase
    {
        public DecisionCase(AppealCase appealCase, CaseBoard workers) 
            : base(appealCase, workers)
        { }

        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursForDecision;
        }

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            registrar.AddToFinishedCaseList(_case);
        }

        internal override void Circulate(Registrar registrar)
        {
            registrar.AddToDecisionCirculation(this);
        }
    }



    internal class OPCase : WorkCase
    {
        public OPCase(AppealCase appealCase, CaseBoard workers) 
            : base(appealCase, workers)
        { }

        protected override int _getWorkHours(CaseWorker worker)
        {
            return worker.HoursOPPreparation;
        }

        internal override void Circulate(Registrar registrar)
        {
            throw new NotImplementedException();
        }

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            WorkCase decisionCase = new DecisionCase(_case, _workers);
            registrar.AddToDecisionCirculation(decisionCase);

        }
    }

}
