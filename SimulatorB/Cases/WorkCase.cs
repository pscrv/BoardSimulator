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

        internal abstract CaseLog Log { get; }
        internal abstract void LogEnqueued(Hour currentHour);

        internal abstract Member GetCurrentMember();

        internal abstract void Work(Hour currentHour, Member member);
        internal abstract void PassToRegistrarIfFinished(Hour currentHour, Member member, Registrar registrar);
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
        protected CaseBoard _caseBoard;

        private Dictionary<Member, Work> _work;                
        private Queue<CaseWorker> _workerQueue;
        #endregion

        #region private properties
        private CaseWorker _currentWorker => _workerQueue.FirstOrDefault();
        private bool _allWorkersAreFinished => _workerQueue.Count < 1;
        #endregion


        #region construction
        internal WorkCaseCommon(AppealCase appealCase, CaseBoard workers)
        {
            if (appealCase == null)
                throw new ArgumentException("appealCase may not be null");
            if (workers == null)
                throw new ArgumentException("workers may not be null");

            _case = appealCase;
            _caseBoard = workers;
            _setupWorkerQueue(workers);
            _setupWork();
        }


        private void _setupWorkerQueue(CaseBoard workers)
        {
            _workerQueue = new Queue<CaseWorker>();
            foreach (CaseWorker worker in workers)
            {
                _workerQueue.Enqueue(worker);
            }
        }

        private void _setupWork()
        {
            _work = new Dictionary<Member, SimulatorB.Work>();
            foreach (CaseWorker worker in _caseBoard)
            {
                _work[worker.Member] = new Work(_getWorkHours(worker));
            }
        }
        #endregion


        #region WorkCase overrides
        internal override CaseWorker Chair => _caseBoard.Chair;
        internal override CaseWorker Rapporteur => _caseBoard.Rapporteur;
        internal override CaseWorker SecondWorker => _caseBoard.SecondWorker;
                
        internal override CaseLog Log => _case.Log;
        internal override void LogEnqueued(Hour currentHour) 
            => Log.LogEnqueued(currentHour, this as dynamic, _currentWorker as dynamic);        

        internal override Member GetCurrentMember() => _currentWorker?.Member;

        internal override void Work(Hour currentHour, Member member)
        {
            CaseWorker worker = _caseBoard.FirstOrDefault(x => x.Member == member);
            if (worker == null)
                throw new InvalidOperationException($"{member} is not allocated to this case.");

            if (_workerQueue.Count < 1)
                throw new InvalidOperationException("No worker queued to do work.");
            
            if (!_work[member].IsStarted)
                Log.LogStarted(currentHour, this as dynamic, worker as dynamic);

            if (!_work[member].IsFinished)
                _work[member].DoWork();
        }


        internal override void PassToRegistrarIfFinished(Hour currentHour, Member member, Registrar registrar)
        {
            if (_work[member].IsFinished)
            {
                Log.LogFinished(currentHour, this as dynamic, _currentWorker as dynamic);
                _workerQueue.Dequeue();

                if (_allWorkersAreFinished)
                {
                    _processFinishedCase(currentHour, registrar);
                }
                else
                {
                    _circulate(registrar);
                }
            }
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
            registrar.ScheduleOP(currentHour, _case, _caseBoard);
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
            Log.LogFinished(currentHour);
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
            return worker.HoursOPPreparation + TimeParameters.OPDurationInHours;
        }

        protected override void _circulate(Registrar registrar)
        { }

        protected override void _processFinishedCase(Hour currentHour, Registrar registrar)
        {
            WorkCase decisionCase = new DecisionCase(_case, _caseBoard);
            registrar.AddToDecisionCirculation(decisionCase);
        }
    }

}
