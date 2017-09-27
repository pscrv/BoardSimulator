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
        internal abstract bool MemberWorkIsFinished { get; }
        internal abstract bool AllWorkersAreFinished { get; }

        internal abstract WorkReport Work(Hour currentHour, Member member);
        
        internal abstract void ProcessFinishedCase(Hour currentHour, Registrar registrar);
        internal abstract void DequeueMember(Member member);


    }



    internal abstract class WorkCaseCommon : WorkCase
    {
        #region abstract
        protected abstract int _getWorkHours(CaseWorker worker);  
        #endregion


        #region fields
        protected AppealCase _case;
        protected CaseBoard _caseBoard;

        private Dictionary<Member, Work> _work;                
        private Queue<CaseWorker> _workerQueue;
        #endregion

        #region private properties
        private CaseWorker _currentWorker => _workerQueue.FirstOrDefault();
        #endregion


        #region construction
        internal WorkCaseCommon(AppealCase appealCase, CaseBoard workers)
        {
            _case = appealCase ?? throw new ArgumentException("appealCase may not be null");
            _caseBoard = workers ?? throw new ArgumentException("workers may not be null");
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

        internal override bool MemberWorkIsFinished => _work[GetCurrentMember()].IsFinished;
        internal override bool AllWorkersAreFinished => _workerQueue.Count < 1;
        

        internal override void LogEnqueued(Hour currentHour) 
            => Log.LogEnqueued(currentHour, this as dynamic, _currentWorker as dynamic);        

        internal override Member GetCurrentMember() => _currentWorker?.Member;

        internal override WorkReport Work(Hour currentHour, Member member)
        {
            CaseWorker worker = _caseBoard.FirstOrDefault(x => x.Member == member);
            if (worker == null)
                throw new InvalidOperationException($"{member} is not allocated to this case.");

            if (_workerQueue.Count < 1)
                throw new InvalidOperationException("No worker queued to do work.");
            
            if (!_work[member].IsStarted)
                Log.LogStarted(currentHour, this as dynamic, worker as dynamic);

            if (_work[member].IsFinished)
            {
                return new NullWorkReport();
            }
            else
            {
                _work[member].DoWork();
                if (_work[member].IsFinished)
                    Log.LogFinished(currentHour, this as dynamic, worker as dynamic);

                return new WorkReport(this as dynamic);
            }

        }

        internal override void DequeueMember(Member member)  // can we lose this?
        {
            if (member != _currentWorker.Member)
                throw new InvalidOperationException("Attempt to dequeue a member who is not the current worker.");

            _workerQueue.Dequeue();
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

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            registrar.ScheduleOP(currentHour, _case, _caseBoard);
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

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            CompletedCaseReport report = new CompletedCaseReport(_case, _caseBoard, Log);
            registrar.AddToFinishedCaseList(report);
            Log.LogFinished(currentHour);
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

        internal override void ProcessFinishedCase(Hour currentHour, Registrar registrar)
        {
            WorkCase decisionCase = new DecisionCase(_case, _caseBoard);
            registrar.AddToDecisionCirculation(decisionCase);
        }
    }

}
