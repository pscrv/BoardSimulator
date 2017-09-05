using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal abstract class OPSchedule
    {
        internal abstract int Count { get; }
        internal abstract List<Hour> StartHours { get; }
        internal abstract void Add(Hour hour, AllocatedCase allocatedCase);
        internal abstract bool HasOPWork(Hour hour, Member member);
        internal abstract AllocatedCase GetOPWork(Hour hour, Member member);
        internal abstract void Schedule(Hour currentHour, AllocatedCase allocatedCase);
        internal abstract List<AllocatedCase> UpdateScheduleAndGetFinishedCases(Hour currentHour);
    }


    internal class OPSchedule1 : OPSchedule
    {
        #region fields and properties
        private int _minimumDaysBetweenOP;
        private Dictionary<Member, Dictionary<Hour, AllocatedCase>> _schedule = new Dictionary<Member, Dictionary<Hour, AllocatedCase>>();
       
        internal override int Count
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


        internal override List<Hour> StartHours
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
        internal OPSchedule1(int minimumDaysBetweenOP)
        {
            _minimumDaysBetweenOP = minimumDaysBetweenOP;
        }

        internal OPSchedule1()
            :this (0) { }
        #endregion


        #region overrides
        internal override void Add(Hour hour, AllocatedCase âllocatedCase)
        {
            foreach (CaseWorker worker in âllocatedCase.Board.MembersAsCaseWorkers)
            {
                if (!_schedule.ContainsKey(worker.Member))
                    _schedule[worker.Member] = new Dictionary<Hour, AllocatedCase>();

                _schedule[worker.Member].Add(hour, âllocatedCase);
            }
        }


        internal override bool HasOPWork(Hour hour, Member member)
        {
            throw new NotImplementedException();
        }

        internal override AllocatedCase GetOPWork(Hour hour, Member member)
        {
            return _getOPWorkAtHour(hour, member);
        }

        internal override void Schedule(Hour currentHour, AllocatedCase allocatedCase)
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
        

        internal override List<AllocatedCase> UpdateScheduleAndGetFinishedCases(Hour currentHour)
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
                finishedList.Add(ac);
            }

            return finishedList;
        }


        internal bool HasOPWork(Hour hour, CaseWorker worker)
        {
            return _hasOPWorkAtHour(hour, worker);
        }
        #endregion


        

        private void _setupForMemberIfNeeded(CaseBoard board)
        {
            foreach (CaseWorker worker in board.MembersAsCaseWorkers)
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
            foreach (CaseWorker worker in allocatedCase.Board.MembersAsCaseWorkers)
            {
                _schedule[worker.Member].Add(startHour, allocatedCase);
            }
        }

        private bool _allFreeForOPDuration(Hour startHour, CaseBoard caseBoard)
        {
            SimulationTimeSpan opDurationSpan = new SimulationTimeSpan(startHour, startHour.AddHours(TimeParameters.OPDurationInHours));
            foreach (CaseWorker worker in caseBoard.MembersAsCaseWorkers)
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
            foreach (CaseWorker worker in caseBoard.MembersAsCaseWorkers)
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
            // TODO: speed this up
            foreach (CaseWorker worker in caseBoard.MembersAsCaseWorkers)
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



    internal class OPSchedule2 : OPSchedule
    {
        #region fields and properties
        private Dictionary<Hour, Dictionary<Member, AllocatedCase>> _memberSchedule;
        private Dictionary<Hour, HashSet<Member>> _blockedSchedule;
        private Dictionary<Hour, List<AllocatedCase>> _startHours;
        private Dictionary<Hour, List<AllocatedCase>> _endHours;

        private int _minimumDaysBetweenOP;
        #endregion



        #region construction
        internal OPSchedule2(int minimumDaysBetweenOP)
        {
            _minimumDaysBetweenOP = minimumDaysBetweenOP;
            
            _memberSchedule = new Dictionary<Hour, Dictionary<Member, AllocatedCase>>();
            _blockedSchedule = new Dictionary<Hour, HashSet<Member>>();
            _startHours = new Dictionary<Hour, List<AllocatedCase>>();
            _endHours = new Dictionary<Hour, List<AllocatedCase>>();
        }

        internal OPSchedule2()
            :this (0) { }
        #endregion


        #region overrides
        internal override void Add(Hour startHour, AllocatedCase allocatedCase)
        {
            Hour endHour = startHour.AddHours(TimeParameters.OPDurationInHours - 1);
            _recordForAllMembers(startHour, endHour, allocatedCase);
            _blockSpanForAllMembers(endHour, allocatedCase);

            if (_startHours.ContainsKey(startHour))
                _startHours[startHour].Add(allocatedCase);
            else
                _startHours[startHour] = new List<AllocatedCase> { allocatedCase };
        }

        internal override bool HasOPWork(Hour hour, Member member)
        {
            if (_memberSchedule.ContainsKey(hour))
            {
                return _memberSchedule[hour].ContainsKey(member);
            }
            
            return false;
        }

        internal override AllocatedCase GetOPWork(Hour hour, Member member)
        {
            if (_memberSchedule.ContainsKey(hour)
                && _memberSchedule[hour].ContainsKey(member))
                return _memberSchedule[hour][member];

            return null;
        }

        //TODO: Should be in the base class, eventually
        private bool IsBlocked(Hour hour, Member member)
        {
            if (_blockedSchedule.ContainsKey(hour)
                && _blockedSchedule[hour].Contains(member))
                return true;

            return false;
        }


        internal override int Count => _startHours.Sum(x => x.Value.Count);

        internal override List<Hour> StartHours => _startHours.Keys.ToList();

        internal override void Schedule(Hour currentHour, AllocatedCase allocatedCase)
        {
            Hour firstPossibleHour = currentHour.AddMonths(TimeParameters.OPMinimumMonthNotice);

            CaseWorker chair = allocatedCase.GetCaseWorkerByRole(WorkerRole.Chair);
            CaseWorker rapporteur = allocatedCase.GetCaseWorkerByRole(WorkerRole.Rapporteur);
            CaseWorker otherWorker = allocatedCase.GetCaseWorkerByRole(WorkerRole.OtherMember);
            
            foreach (
                SimulationTimeSpan chairFreeSpan in 
                    _getFreeIntervals(firstPossibleHour, chair.Member))           
            {
                foreach (Hour possibleStart in _firstHoursOfDays(chairFreeSpan))
                {
                    if (
                        _isPossibleStartForCaseWorker(possibleStart, chair)
                        && _isPossibleStartForCaseWorker(possibleStart, rapporteur)
                        && _isPossibleStartForCaseWorker(possibleStart, otherWorker))
                    {
                        Add(possibleStart, allocatedCase);
                        return;
                    }
                }
            }

            throw new InvalidOperationException("Could not schedule OP.");
        }

        internal override List<AllocatedCase> UpdateScheduleAndGetFinishedCases(Hour currentHour)
        {
            _processStartingCases(currentHour);
            _removeEntries(_memberSchedule, _memberSchedule.Keys.Where(x => x <= currentHour).ToList());
            _removeEntries(_blockedSchedule, _blockedSchedule.Keys.Where(x => x <= currentHour).ToList());
            return _processAndGetFinishedCases(currentHour);
        }
        #endregion





        #region private methods       
        private void _recordForAllMembers(Hour startHour, Hour endHour, AllocatedCase allocatedCase)
        {
            _recordForMember(startHour, endHour, allocatedCase, WorkerRole.Chair);
            _recordForMember(startHour, endHour, allocatedCase, WorkerRole.Rapporteur);
            _recordForMember(startHour, endHour, allocatedCase, WorkerRole.OtherMember);
        }

        private void _recordForMember(Hour startHour, Hour endHour, AllocatedCase allocatedCase, WorkerRole role)
        {
            Member member = allocatedCase.GetMemberByRole(role);
            CaseWorker worker = allocatedCase.GetCaseWorkerByRole(role);
            Hour preparationStart = startHour.SubtractHours(worker.HoursOPPreparation);
            SimulationTimeSpan opSpan = new SimulationTimeSpan(preparationStart, endHour);

            foreach (Hour hour in opSpan)
            {
                if (_blockedSchedule.ContainsKey(hour) && _blockedSchedule[hour].Contains(worker.Member))
                    throw new InvalidOperationException(
                        string.Format("OPSchedule2.Add: attempt to schedule but member is blocked for {0}", hour));

                _addOPScheduleForWorker(hour, allocatedCase, worker);
            }
        }
       

        private void _addOPScheduleForWorker(Hour hour, AllocatedCase allocatedCase, CaseWorker worker)
        {
            if (_memberSchedule.ContainsKey(hour))
            {
                if (_memberSchedule[hour].ContainsKey(worker.Member))
                    throw new InvalidOperationException(
                        $"OPSchedule2._testAddOPDataScheduleForMember: member has already present for {hour}");

                _memberSchedule[hour][worker.Member] = allocatedCase;
            }
            else
            {
                _memberSchedule[hour] = new Dictionary<Member, AllocatedCase>
                {
                    [worker.Member] = allocatedCase
                };
            }
        }


        private void _blockSpanForAllMembers(Hour endHour, AllocatedCase allocatedCase)
        {
            _blockSpanForOneMember(endHour, allocatedCase.GetMemberByRole(WorkerRole.Chair));
            _blockSpanForOneMember(endHour, allocatedCase.GetMemberByRole(WorkerRole.Rapporteur));
            _blockSpanForOneMember(endHour, allocatedCase.GetMemberByRole(WorkerRole.OtherMember));
        }

        private void _blockSpanForOneMember(Hour currentHour, Member member)
        {
            if (_minimumDaysBetweenOP == 0)
                return;

            Hour blockStart = currentHour.Next();
            Hour blockEnd = blockStart.NextFirstHourOfDay().AddDays(_minimumDaysBetweenOP);
            SimulationTimeSpan blockSpan = new SimulationTimeSpan(blockStart, blockEnd);

            foreach (Hour hour in blockSpan)
            {
                _blockHourForMember(member, hour);
            }
        }

        private void _blockHourForMember(Member member, Hour hour)
        {
            if (_blockedSchedule.ContainsKey(hour))
            {
                if (_blockedSchedule[hour].Contains(member))
                    throw new InvalidOperationException(
                        string.Format("OPSchedule2.Add: member has already present for {0}", hour));

                _blockedSchedule[hour].Add(member);
            }
            else
            {
                _blockedSchedule[hour] = new HashSet<Member> { member };
            }
        }
        

        private bool _isPossibleStartForCaseWorker(Hour possibleStart, CaseWorker worker)
        {
            SimulationTimeSpan span =
                new SimulationTimeSpan(
                    possibleStart.SubtractHours(worker.HoursOPPreparation),
                    possibleStart.AddHours(TimeParameters.OPDurationInHours));

            foreach (Hour hour in span)
            {
                if (HasOPWork(hour, worker.Member) || IsBlocked(hour, worker.Member))
                    return false;
            }

            return true;
        }


        //TODO: consider moving to SimulationTimespan
        private IEnumerable<Hour> _firstHoursOfDays(SimulationTimeSpan span)
        {
            Hour hour = span.Start.NextFirstHourOfDay();
            while (hour <= span.End)
            {
                yield return hour;
                hour = hour.AddDays(1);
            }
        }

        private IEnumerable<SimulationTimeSpan> _getFreeIntervals(Hour startHour, Member member)
        {
            SimulationTimeSpan span = _getNextFreeInterval(startHour, member);
            yield return span;

            while (span.End != Hour.MaxHour)
            {
                span = _getNextFreeInterval(span.End.Next(), member);
                yield return span;
            }
        }
        
        private SimulationTimeSpan _getNextFreeInterval(Hour startHour, Hour endHour, Member member)
        {
            Hour membersLastBlockedHour = _blockedSchedule
                .Where(x => x.Value.Contains(member))
                .Max(x => x.Key);


            Hour start = _nextFreeHour(startHour, member);
            if (endHour < start)
                return null;

            if (membersLastBlockedHour == null || membersLastBlockedHour < start)
                return new SimulationTimeSpan(start, null);

            Hour end = _nextBusyHour(start, member);
            return new SimulationTimeSpan(start, end.Previous());
        }

        private SimulationTimeSpan _getNextFreeInterval(Hour startHour, Member member)
        {
            return _getNextFreeInterval(startHour, Hour.MaxHour, member);
        }

        private Hour _nextBusyHour(Hour hour, Member member)
        {
            while (
                (_memberSchedule.ContainsKey(hour) && !_memberSchedule[hour].ContainsKey(member))
                || !_memberSchedule.ContainsKey(hour))
            {
                hour = hour.Next();
            }

            return hour;
        }

        private Hour _nextFreeHour(Hour hour, Member member)
        {
            while (
                _memberSchedule.ContainsKey(hour) && _memberSchedule[hour].ContainsKey(member)
                || _blockedSchedule.ContainsKey(hour) && _blockedSchedule[hour].Contains(member))
            {
                hour = hour.Next();
            }

            return hour;
        }



        private void _processStartingCases(Hour currentHour)
        {
            if (_startHours.ContainsKey(currentHour))
            {
                _recordOPStarts(currentHour);
                _scheduleEndHours(currentHour);
                _startHours.Remove(currentHour);
            }
        }

        private List<AllocatedCase> _processAndGetFinishedCases(Hour currentHour)
        {
            List<AllocatedCase> finishedCases = new List<AllocatedCase>();
            if (_endHours.ContainsKey(currentHour))
            {
                _recodOPFinishes(currentHour);
                finishedCases = _endHours[currentHour];
                _endHours.Remove(currentHour);
            }

            return finishedCases;
        }

        private void _recordOPStarts(Hour currentHour)
        {
            foreach (AllocatedCase ac in _startHours[currentHour])
            {
                ac.Record.SetOPStart(currentHour);
            }
        }

        private void _recodOPFinishes(Hour currentHour)
        {
            foreach (AllocatedCase ac in _endHours[currentHour])
            {
                ac.Record.SetOPFinished(currentHour);
            }
        }

        private void _scheduleEndHours(Hour currentHour)
        {
            _endHours
                [currentHour.AddHours(TimeParameters.OPDurationInHours)]
                = _startHours[currentHour];
        }

        private void _removeEntries<S, T>(Dictionary<S, T> dictionary,  List<S> toRemove)
        {
            foreach (S s in toRemove)
            {
                dictionary.Remove(s);
            }
        }
        #endregion
    }
}