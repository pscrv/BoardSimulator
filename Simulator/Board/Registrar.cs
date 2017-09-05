
using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Registrar
    {
        #region fields and properties
        private BoardQueue _boardQueue;
        private IncomingCaseQueue _incoming;
        private CirculationQueue _circulation;
        private OPSchedule _opSchedule;
        private FinishedCaseList _finished;


        internal List<AllocatedCase> FinishedCases { get { return _finished.Cases; } }
        #endregion


        #region construction
        internal Registrar()
        {
            _boardQueue = new BoardQueue();
            _incoming = new IncomingCaseQueue();
            _circulation = new CirculationQueue();
            //_opSchedule = new OPSchedule1();
            _opSchedule = new OPSchedule2();
            _finished = new FinishedCaseList();
        }

        internal Registrar(OPSchedule opSchedule)
        {
            _boardQueue = new BoardQueue();
            _incoming = new IncomingCaseQueue();
            _circulation = new CirculationQueue();
            _opSchedule = opSchedule;
            _finished = new FinishedCaseList();
        }
        #endregion




        internal void RegisterMember(Member member)
        {
            _boardQueue.Register(member);
        }


        internal void DoWork(Hour currentHour)
        {
            _incoming.EnqueueForNextStage(currentHour);
            
            foreach (AllocatedCase finishedCase in _opSchedule.UpdateScheduleAndGetFinishedCases(currentHour))
            {
                finishedCase.EnqueueForWork(currentHour);
            }

            _circulation.EnqueueForNextStage(currentHour);
        }


        internal AllocatedCase GetCurrentCase(Hour currentHour, Member member)
        {
            return _opSchedule.GetOPWork(currentHour, member) ?? _boardQueue.Peek(member);
        }


        internal void ProcessFinishedWork(Hour currentHour, AllocatedCase allocatedCase, Member member)
        {
            _circulation.Enqueue(currentHour, allocatedCase);
            _boardQueue.Dequeue(member);
            if (allocatedCase.Stage == CaseStage.Finished)
            {
                _finished.Add(allocatedCase);
            }
        }


        internal void ProcessIncomingCase(Hour currentHour, AllocatedCase allocatedCase)
        {
            _incoming.Enqueue(currentHour, allocatedCase);
        }


        internal void AddToCirculation(Hour currentHour, AllocatedCase allocatedCase)
        {
            _circulation.Enqueue(currentHour, allocatedCase);
        }


        internal void EnqueueForMember(Hour currentHour, CaseWorker nextWorker, AllocatedCase allocatedCase)
        {
            _boardQueue.EnqueueForMember(currentHour, nextWorker, allocatedCase);
        }



        internal int MemberQueueCount(Member member)
        {
            return _boardQueue.Count(member);
        }

        internal int CirculationQueueCount()
        {
            return _circulation.Count;
        }

        internal int OPScheduleCount()
        {
            return _opSchedule.Count;
        }

        internal void ScheduleOP(Hour currentHour, AllocatedCase allocatedCase)
        {
            _opSchedule.Schedule(currentHour, allocatedCase);
        }
    }
}