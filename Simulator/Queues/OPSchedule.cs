using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class OPSchedule
    {
        #region fields and properties
        private int _minimumDaysBetweenOP;
        private Dictionary<Member, Dictionary<Hour, AllocatedCase>> _schedule = new Dictionary<Member, Dictionary<Hour, AllocatedCase>>();
       
        internal int Count
        {
            get
            {
                int count = 0;
                foreach (Member member in _schedule.Keys)
                {
                    count += _schedule[member].Count;
                }
                return count;
            }
        }


        internal List<Hour> StartHours
        {
            get
            {
                HashSet<Hour> result = new HashSet<Hour>();
                foreach (Member member in _schedule.Keys)
                {
                    foreach (Hour hour in _schedule[member].Keys)
                    {
                        result.Add(hour);
                    }
                }

                return result.ToList();
            }
        }


        internal List<AllocatedCase> ScheduledCases
        {
            get
            {
                HashSet<AllocatedCase> result = new HashSet<AllocatedCase>();
                foreach (Member member in _schedule.Keys)
                {
                    foreach (Hour hour in _schedule[member].Keys)
                    {
                        result.Add(_schedule[member][hour]);
                    }
                }

                return result.ToList();
            }
        }
        #endregion



        #region construction
        internal OPSchedule(int minimumDaysBetweenOP)
        {
            _minimumDaysBetweenOP = minimumDaysBetweenOP;
        }

        internal OPSchedule()
            :this (0) { }
        #endregion


        internal void Add(Hour hour, AllocatedCase âllocatedCase)
        {
            foreach (CaseWorker worker in âllocatedCase.Board.Members)
            {
                if (!_schedule.ContainsKey(worker.Member))
                    _schedule[worker.Member] = new Dictionary<Hour, AllocatedCase>();

                _schedule[worker.Member].Add(hour, âllocatedCase);
            }
        }


        internal bool HasOPWork(Hour hour, CaseWorker worker)
        {
            return _hasOPWorkAtHour(hour, worker);
        }

        internal AllocatedCase GetOPWork(Hour hour, Member member)
        {
            return _getOPWorkAtHour(hour, member);
        }
        

        internal void Schedule(Hour currentHour, AllocatedCase allocatedCase)
        {
            _setupForMemberIfNeeded(allocatedCase.Board);

            Hour earliestPossibleHour =
                currentHour.NextFirstHourOfDay().AddMonths(TimeParameters.OPMinimumMonthNotice);
            Hour iterationHour = earliestPossibleHour;

            while (true)
            {
                iterationHour = _nextFirstHourOfDayWhenAllFree(allocatedCase.Board, iterationHour);

                if (_isEnoughTimeSincePreviousOP(iterationHour, allocatedCase.Board)
                    && _allFreeForOPDuration(iterationHour, allocatedCase.Board)
                    && _allHaveEnoughPreprationTime(iterationHour, allocatedCase.Board)
                    && _isEnoughTimeBeforeNextOP(iterationHour, allocatedCase.Board))
                {
                    _scheduleForAllMembers(iterationHour, allocatedCase);
                    return;
                }

                iterationHour = iterationHour.Next();
            }
        }


        internal List<AllocatedCase> UpdateScheduleAndGetFinishedCases(Hour currentHour)
        {
            HashSet<AllocatedCase> startedCases = new HashSet<AllocatedCase>();
            HashSet<AllocatedCase> finishedCases = new HashSet<AllocatedCase>();
            List<Tuple<Member, Hour>> toRemove = new List<Tuple<Member, Hour>>();

            foreach (Member member in _schedule.Keys)
            {
                foreach (Hour hour in _schedule[member].Keys)
                {
                    if (hour == currentHour)
                    {
                        startedCases.Add(_schedule[member][hour]);
                    }

                    if (hour.AddHours(TimeParameters.OPDurationInHours - 1) < currentHour)
                    {
                        finishedCases.Add(_schedule[member][hour]);
                        toRemove.Add(new Tuple<Member, Hour>( member, hour ));
                    }
                }

            }

            foreach (AllocatedCase ac in startedCases)
            {
                ac.Record.SetOPStart(currentHour);
            }

            foreach (Tuple<Member, Hour> item in toRemove)
            {
                _schedule[item.Item1].Remove(item.Item2);
            }

            List<AllocatedCase> finishedList = new List<AllocatedCase>();
            foreach (AllocatedCase ac in finishedCases)
            {
                ac.Record.SetOPFinished(currentHour);
                //_circulation.Enqueue(currentHour, ac);
                finishedList.Add(ac);
            }

            return finishedList;
        }






        private void _setupForMemberIfNeeded(CaseBoard board)
        {
            foreach (CaseWorker worker in board.Members)
            {
                if (!_schedule.ContainsKey(worker.Member))
                    _schedule[worker.Member] = new Dictionary<Hour, AllocatedCase>();
            }
        }

        private Hour _nextFirstHourOfDayWhenAllFree(CaseBoard board, Hour startHour)
        {
            Hour iterationHour = startHour.NextFirstHourOfDay();
            while (_someMemberIsbusyAtHour(iterationHour, board))
            {
                iterationHour = iterationHour.FirstHourOfNextDay();
            }

            return iterationHour;
        }

        private bool _someMemberIsbusyAtHour(Hour hour, CaseBoard board)
        {
            return (_hasOPWorkAtHour(hour, board.Chair)
                    || _hasOPWorkAtHour(hour, board.Rapporteur)
                    || _hasOPWorkAtHour(hour, board.OtherMember));
        }

        private void _scheduleForAllMembers(Hour startHour, AllocatedCase allocatedCase)
        {
            foreach (CaseWorker worker in allocatedCase.Board.Members)
            {
                _schedule[worker.Member].Add(startHour, allocatedCase);
            }
        }

        private bool _allFreeForOPDuration(Hour startHour, CaseBoard caseBoard)
        {
            SimulationTimeSpan opDurationSpan = new SimulationTimeSpan(startHour, startHour.AddHours(TimeParameters.OPDurationInHours));
            foreach (CaseWorker worker in caseBoard.Members)
            {
                foreach (Hour hour in opDurationSpan)
                {
                    if (_hasOPWorkAtHour(hour, worker))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool _allHaveEnoughPreprationTime(Hour startHour, CaseBoard caseBoard)
        {
            foreach (CaseWorker worker in caseBoard.Members)
            {
                SimulationTimeSpan preparationSpan = 
                    new SimulationTimeSpan(
                        startHour.SubtractHours(worker.HoursOPPreparation), 
                        startHour.Previous());

                foreach (Hour hour in preparationSpan)
                {
                    if (_hasOPWorkAtHour(hour, worker))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool _hasOPWorkAtHour(Hour hour, CaseWorker worker)
        {
            return _getOPWorkAtHour(hour, worker.Member) != null;
        }

        private AllocatedCase _getOPWorkAtHour(Hour hour, Member member)
        {
            if (member == null)
                throw new NullReferenceException("OPSchedule.HasOPWork: parameter <worker> is null.");

            if (!_schedule.ContainsKey(member))
                return null;

            Hour lastStartBeforeHour =
                 (from h in _schedule[member].Keys
                  where h < hour
                  select h).Max();

            if (lastStartBeforeHour != null)
            {
                Hour busyUntil = lastStartBeforeHour
                    .AddHours(TimeParameters.OPDurationInHours - 1);
                if (busyUntil >= hour)
                {
                    return _schedule[member][lastStartBeforeHour];
                }
            }

            Hour nextStartTime =
                (from h in _schedule[member].Keys
                 where h >= hour
                 select h).Min();
            if (nextStartTime == null)
                return null;

            WorkerRole roleInNextCase = _schedule[member][nextStartTime].Board.GetRole(member);
            int hoursOfPreparation = member.GetParameters(roleInNextCase).HoursOPPrepration;

            Hour nextWorkStartHour = nextStartTime.SubtractHours(hoursOfPreparation);
            if (nextWorkStartHour <= hour)
                return _schedule[member][nextStartTime];

            return null;
        }


        private bool _isEnoughTimeBeforeNextOP(Hour hour, CaseBoard caseBoard)
        {
            Hour needsToBeFreeUntilHour =
                hour.AddHours(TimeParameters.OPDurationInHours)
                    .NextFirstHourOfDay()
                    .AddDays(_minimumDaysBetweenOP);

            SimulationTimeSpan span = new SimulationTimeSpan(hour, needsToBeFreeUntilHour);

            return _allMembersFreeForSpan(caseBoard, span);
        }

        private bool _allMembersFreeForSpan(CaseBoard caseBoard, SimulationTimeSpan span)
        {
            foreach (CaseWorker worker in caseBoard.Members)
            {
                foreach (Hour h in span)
                {
                    if (_hasOPWorkAtHour(h, worker))
                        return false;
                }
            }

            return true;
        }

        private bool _isEnoughTimeSincePreviousOP(Hour hour, CaseBoard caseBoard)
        {
            Hour needsToBeFreeFromHour = hour.SubtractDays(_minimumDaysBetweenOP);
            SimulationTimeSpan span = new SimulationTimeSpan(needsToBeFreeFromHour, hour);
            return _allMembersFreeForSpan(caseBoard, span);
        }
    }
}