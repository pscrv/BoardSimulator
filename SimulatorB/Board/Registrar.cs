
using System;
using System.Collections.Generic;

namespace SimulatorB
{
    internal abstract class Registrar
    {
        internal abstract int CirculatingSummonsCount { get; }
        internal abstract int CirculatingDecisionsCount { get; }
        internal abstract int PendingOPCount { get; }
        internal abstract int RunningOPCount { get; }
        internal abstract int FinishedCaseCount { get; }

        internal abstract void AddToSummonsCirculation(WorkCase summonsCase);
        internal abstract void AddToDecisionCirculation(WorkCase decisionCase);
        internal abstract void AddToFinishedCaseList(AppealCase finishedCase);
        internal abstract WorkCase GetMemberWork(Hour currentHour, Member member);
        internal abstract void CirculateCases(Hour currentHour);
        internal abstract void UpdateOPSchedule(Hour currentHour);
        internal abstract void ScheduleOP(Hour currentHour, AppealCase appealCase, CaseBoard workers);
        internal abstract bool MemberHasOP(Hour currentHour, Member member);
    }


    internal class BasicRegistrar : Registrar
    {
        #region fields
        private Queue<WorkCase> _circulatingSummonses;
        private Queue<WorkCase> _circulatingDecisions;
        private Dictionary<Member, Queue<WorkCase>> _summonsQueues;
        private Dictionary<Member, Queue<WorkCase>> _decisionQueues;
        private OPSchedule _opSchedule;
        private List<AppealCase> _finishedCases;  
        #endregion


        #region property overrides
        internal override int CirculatingSummonsCount => _circulatingSummonses.Count;
        internal override int CirculatingDecisionsCount => _circulatingDecisions.Count;
        internal override int PendingOPCount => _opSchedule.Count;
        internal override int RunningOPCount => _opSchedule.RunningCases.Count;
        internal override int FinishedCaseCount => _finishedCases.Count;
        #endregion



        #region construction
        internal BasicRegistrar(IEnumerable<Member> members)
        {
            _circulatingSummonses = new Queue<WorkCase>();
            _circulatingDecisions = new Queue<WorkCase>();
            _summonsQueues = new Dictionary<Member, Queue<WorkCase>>();
            _decisionQueues = new Dictionary<Member, Queue<WorkCase>>();
            foreach (Member member in members)
            {
                _summonsQueues[member] = new Queue<WorkCase>();
                _decisionQueues[member] = new Queue<WorkCase>();
            }
            _opSchedule = new SimpleOPScheduler();
            _finishedCases = new List<AppealCase>();
        }
        #endregion



        #region internal override methods
        internal override void AddToSummonsCirculation(WorkCase summonsCase)
        {
            _circulatingSummonses.Enqueue(summonsCase);
        }

        internal override void AddToDecisionCirculation(WorkCase decisionCase)
        {
            _circulatingDecisions.Enqueue(decisionCase);
        }

        internal override void AddToFinishedCaseList(AppealCase finishedCase)
        {
            _finishedCases.Add(finishedCase);
        }


        internal override WorkCase GetMemberWork(Hour currentHour, Member member)
        {
            if (MemberHasOP(currentHour, member))
                return _opSchedule.GetOPWork(currentHour, member);

            if (_decisionQueues[member].Count > 0)
                return _decisionQueues[member].Dequeue();

            if (_summonsQueues[member].Count > 0)
                return _summonsQueues[member].Dequeue();

            return null;
        }


        internal override void CirculateCases(Hour currentHour)
        {
            _circulateFromQueue(_circulatingSummonses, _summonsQueues, currentHour);
            _circulateFromQueue(_circulatingDecisions, _decisionQueues, currentHour);
            // new cases too
        }

        internal override void UpdateOPSchedule(Hour currentHour)
        {
            _opSchedule.UpdateSchedule(currentHour);    
        }


        internal override void ScheduleOP(Hour currentHour, AppealCase appealCase, CaseBoard workers)
        {
            WorkCase opCase = new OPCase(appealCase, workers);
            opCase.LogEnqueued(currentHour);
            _opSchedule.Schedule(currentHour, appealCase, workers);
        }
        


        internal override bool MemberHasOP(Hour currentHour, Member member)
        {
            return _opSchedule.HasOPWork(currentHour, member);
        }
        #endregion        



        private void _circulateFromQueue(
            Queue<WorkCase> inqueue, 
            Dictionary<Member, Queue<WorkCase>> outqueues,
            Hour currentHour)
        {
            while (inqueue.Count > 0)
            {
                var workCase = inqueue.Dequeue();
                var workingMember = workCase.GetCurrentMember();
                outqueues[workingMember].Enqueue(workCase);
                workCase.LogEnqueued(currentHour);
            }
        }
        
    }
}