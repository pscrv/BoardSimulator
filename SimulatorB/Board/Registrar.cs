
using System;
using System.Collections.Generic;

namespace SimulatorB
{
    internal class Registrar
    {
        #region fields
        private Queue<WorkCase> _circulatingSummonses;
        private Queue<WorkCase> _circulatingDecisions;
        private Dictionary<Member, Queue<WorkCase>> _summonsQueues;
        private Dictionary<Member, Queue<WorkCase>> _decisionQueues;
        private OPSchedule _opSchedule;
        private List<AppealCase> _finishedCases;  // will need some other form, this for now
        #endregion


        #region properties
        internal int CirculatingSummonsCount => _circulatingSummonses.Count;
        internal int CirculatingDecisionsCount => _circulatingDecisions.Count;
        internal int PendingOPCount => _opSchedule.Count;
        internal int RunningOPCount => _opSchedule.RunningCases.Count;
        internal int FinishedCaseCount => _finishedCases.Count;
        #endregion



        #region construction
        public Registrar(IEnumerable<Member> members)
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



        #region internal methods
        internal void AddToSummonsCirculation(WorkCase summonsCase)
        {
            _circulatingSummonses.Enqueue(summonsCase);
        }

        internal void AddToDecisionCirculation(WorkCase decisionCase)
        {
            _circulatingDecisions.Enqueue(decisionCase);
        }

        internal void AddToFinishedCaseList(AppealCase finishedCase)
        {
            _finishedCases.Add(finishedCase);
        }


        internal WorkCase GetMemberWork(Member member)
        {
            if (_decisionQueues[member].Count > 0)
                return _decisionQueues[member].Dequeue();

            if (_summonsQueues[member].Count > 0)
                return _summonsQueues[member].Dequeue();

            return null;
        }


        internal void DoWork(Hour currentHour)
        {
            ProcessCirculatingCases();
            _opSchedule.UpdateSchedule(currentHour);
            foreach (WorkCase workCase in _opSchedule.FinishedCases)
            {
                workCase.ProcessFinishedCase(currentHour, this);
            }
        }


        internal void ProcessCirculatingCases()
        {
            _circulateFromQueue(_circulatingSummonses, _summonsQueues);
            _circulateFromQueue(_circulatingDecisions, _decisionQueues);
            // new cases too
        }



        internal void ScheduleOP(Hour currentHour, AppealCase appealCase, CaseBoard workers)
        {
            _opSchedule.Schedule(currentHour, appealCase, workers);
        }


        internal bool MemberHasOP(Hour currentHour, Member member)
        {
            return _opSchedule.HasOPWork(currentHour, member);
        }
        #endregion        


        private void _circulateFromQueue(Queue<WorkCase> inqueue, Dictionary<Member, Queue<WorkCase>> outqueues)
        {
            while (inqueue.Count > 0)
            {
                var workCase = inqueue.Dequeue();
                var workingMember = workCase.GetNextMember();
                outqueues[workingMember].Enqueue(workCase);
            }
        }
        
    }
}