using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class OPSchedule
    {
        #region fields and properties
        private Dictionary<Member, Dictionary<Hour, AllocatedCase>> _schedule = new Dictionary<Member, Dictionary<Hour, AllocatedCase>>();
        private CirculationQueue _circulation;
        
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
        internal OPSchedule(CirculationQueue circulation)
        {
            _circulation = circulation;
        }
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
        

        internal void Schedule(Hour currentHour, AllocatedCase allocateCase)
        {
            _setupForMemberIfNeeded(allocateCase.Board);


            Hour earliestPossibleHour =
                currentHour.AddMonths(TimeParameters.OPMinimumMonthNotice);
            Hour iterationHour = earliestPossibleHour;

            while (true)
            {
                iterationHour = _nextFirstHourOfDayWhenAllFree(allocateCase.Board, iterationHour);

                if (_allFreeForOPDuration(iterationHour, allocateCase.Board)
                    && _allHaveEnoughPreprationTime(iterationHour, allocateCase.Board))
                {
                    _scheduleForAllMembers(iterationHour, allocateCase);
                    return;
                }
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
            Hour iterationHour = startHour.FirstHourOfNextDay();
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
            if (worker == null)
                throw new NullReferenceException("OPSchedule.HasOPWork: parameter <worker> is null.");

            if (!_schedule.ContainsKey(worker.Member))
                return false;
            
            Hour lastStartBeforeHour = 
                 (from h in _schedule[worker.Member].Keys
                  where h < hour
                  select h).Max();

            if (lastStartBeforeHour != null)
            {
                if (lastStartBeforeHour.AddHours(TimeParameters.OPDurationInHours - 1) >= hour)
                    return true;
            }

            Hour nextStartTime =
                (from h in _schedule[worker.Member].Keys
                 where h >= hour
                 select h).Min();
            if (nextStartTime == null)
                return false;

            Hour nextWorkStartHour = nextStartTime.SubtractHours(worker.HoursOPPreparation);
            if (nextWorkStartHour <= hour)
                return true;

            return false;
        }


        
    }
}