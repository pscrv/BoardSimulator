
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    internal abstract class Registrar
    {
        internal abstract int CirculatingSummonsCount { get; }
        internal abstract int CirculatingDecisionsCount { get; }
        internal abstract int PendingOPCount { get; }
        internal abstract int RunningOPCount { get; }
        internal abstract int FinishedCaseCount { get; }

        internal abstract void ProcessNewSummons(WorkCase summonsCase);
        internal abstract void AddToDecisionCirculation(WorkCase decisionCase);
        internal abstract void CirculateCases(Hour currentHour);
        internal abstract void AddToFinishedCaseList(AppealCase finishedCase);
        internal abstract WorkCase GetMemberWork(Hour currentHour, Member member);
        internal abstract void ScheduleOP(Hour currentHour, AppealCase appealCase, CaseBoard workers);
        internal abstract void UpdateQueuesAndCirculate(Hour currentHour);
    }


    internal class BasicRegistrar : Registrar
    {
        #region fields
        private List<Member> _members;
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
            _members = members.ToList();
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
        internal override void ProcessNewSummons(WorkCase summonsCase)
        {
            _addToSummonsCirculation(summonsCase);
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
            if (_memberHasOP(currentHour, member))
                return _opSchedule.GetOPWork(currentHour, member);

            if (_decisionQueues[member].Count > 0)
                return _decisionQueues[member].Peek();

            if (_summonsQueues[member].Count > 0)
                return _summonsQueues[member].Peek();

            return null;
        }


        internal override void CirculateCases(Hour currentHour)
        {
            _circulateFromQueue(_circulatingSummonses, _summonsQueues, currentHour);
            _circulateFromQueue(_circulatingDecisions, _decisionQueues, currentHour);
            //// new cases too
        }

        internal override void ScheduleOP(Hour currentHour, AppealCase appealCase, CaseBoard workers)
        {
            WorkCase opCase = new OPCase(appealCase, workers);
            opCase.LogEnqueued(currentHour);
            _opSchedule.Schedule(currentHour, appealCase, workers);
        }
        



        internal override void UpdateQueuesAndCirculate(Hour currentHour)
        {
            _updateOPSchedule(currentHour);
            _processFinishedWork(currentHour);
            CirculateCases(currentHour);
        }
        #endregion



        private void _addToSummonsCirculation(WorkCase summonsCase)
        {
            _circulatingSummonses.Enqueue(summonsCase);
        }

        private bool _memberHasOP(Hour currentHour, Member member)
        {
            return _opSchedule.HasOPWork(currentHour, member);
        }

        private void _circulateFromQueue(
            Queue<WorkCase> inqueue, 
            Dictionary<Member, Queue<WorkCase>> outqueues,
            Hour currentHour)
        {
            while (inqueue.Count > 0)
            {
                WorkCase workCase = inqueue.Dequeue();
                Member workingMember = workCase.GetCurrentMember();
                outqueues[workingMember].Enqueue(workCase);
                workCase.LogEnqueued(currentHour);
            }
        }       


        private void _updateOPSchedule(Hour currentHour)
        {
            _opSchedule.UpdateSchedule(currentHour);    
        }
        
        private void _processFinishedOPs(Hour currentHour)
        {
            foreach (WorkCase opCase in _opSchedule.FinishedCases)
            {
                opCase.ProcessFinishedCase(currentHour, this);
            }
        }


        private void _processFinishedWork(Hour currentHour)
        {
            _processFinishedOPs(currentHour);

            foreach (Member member in _members)
            {
                _processFinished(currentHour, member, _summonsQueues[member]);
                _processFinished(currentHour, member, _decisionQueues[member]);
            }
        }

        private void _processFinished(Hour currentHour, Member member, Queue<WorkCase> queue)
        {
            WorkCase membercase;
            if (queue.Count > 0)
            {
                membercase = queue.Peek();
                if (membercase.MemberWorkIsFinished)
                {
                    membercase.DequeueMember(member);
                    queue.Dequeue();

                    if (membercase.AllWorkersAreFinished)
                    {
                        membercase.ProcessFinishedCase(currentHour, this);
                    }
                    else
                    {
                        _addToSummonsCirculation(membercase);
                    }
                }
            }
        }


    }
}