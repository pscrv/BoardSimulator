using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    internal abstract class OPSchedule
    {
        internal abstract int Count { get; }
        internal abstract List<Hour> StartHours { get; }
        internal abstract List<WorkCase> RunningCases { get; }
        internal abstract List<WorkCase> StartedCases { get; }
        internal abstract List<WorkCase> FinishedCases { get; }

        internal abstract void Add(Hour hour, WorkCase WorkCase);
        internal abstract bool HasOPWork(Hour hour, Member member);
        internal abstract void Schedule(Hour startHour, AppealCase appealCase, CaseBoard caseBoard);
        internal abstract void UpdateSchedule(Hour currentHour);
        //internal abstract bool IsBlocked(Hour hour, Member member);
    }


    internal class SimpleOPScheduler : OPSchedule
    {
        #region fields and properties
        private int _minimumDaysBetweenOP;
        private Dictionary<Hour, Dictionary<Member, WorkCase>> _memberSchedule;
        private Dictionary<Hour, HashSet<Member>> _blockedSchedule;
        private Dictionary<Hour, List<WorkCase>> _startHours;
        private Dictionary<Hour, List<WorkCase>> _endHours;
        
        Hour _lastUpdateHour;
        private List<WorkCase> _startedCases;
        private List<WorkCase> _finishedCases;
        internal override List<WorkCase> StartedCases { get => _startedCases; }
        internal override List<WorkCase> FinishedCases { get => _finishedCases; }
        #endregion



        #region construction
        internal SimpleOPScheduler(int minimumDaysBetweenOP)
        {
            _minimumDaysBetweenOP = minimumDaysBetweenOP;
            
            _memberSchedule = new Dictionary<Hour, Dictionary<Member, WorkCase>>();
            _blockedSchedule = new Dictionary<Hour, HashSet<Member>>();
            _startHours = new Dictionary<Hour, List<WorkCase>>();
            _endHours = new Dictionary<Hour, List<WorkCase>>();

            _lastUpdateHour = null;
            _startedCases = new List<WorkCase>();
            _finishedCases = new List<WorkCase>();
        }

        internal SimpleOPScheduler()
            :this (0) { }
        #endregion


        #region overrides
        internal override void Add(Hour startHour, WorkCase WorkCase)
        {
            Hour endHour = startHour.AddHours(TimeParameters.OPDurationInHours - 1);
            _recordForAllMembers(startHour, endHour, WorkCase);
            _blockSpanForAllMembers(endHour, WorkCase);

            if (_startHours.ContainsKey(startHour))
                _startHours[startHour].Add(WorkCase);
            else
                _startHours[startHour] = new List<WorkCase> { WorkCase };
        }

        internal override bool HasOPWork(Hour hour, Member member)
        {
            if (_memberSchedule.ContainsKey(hour))
            {
                return _memberSchedule[hour].ContainsKey(member);
            }
            
            return false;
        }
        

        
        internal override int Count => _startHours.Sum(x => x.Value.Count);

        internal override List<Hour> StartHours => _startHours.Keys.ToList();


        internal override List<WorkCase> RunningCases
        {
            get => _endHours.Values.Aggregate(
                new List<WorkCase>(), 
                (a, b) => a.Concat(b).ToList());
        }



        internal override void Schedule(Hour currentHour, AppealCase appealCase, CaseBoard caseBoard)
        {
            Hour firstPossibleHour =
                currentHour
                .NextFirstHourOfDay()
                .AddMonths(TimeParameters.OPMinimumMonthNotice);

            CaseWorker chair = caseBoard.Chair;
            CaseWorker rapporteur = caseBoard.Rapporteur;
            CaseWorker secondWorker = caseBoard.SecondWorker;

            foreach (
                SimulationTimeSpan chairFreeSpan in
                    _getFreeIntervals(firstPossibleHour, chair.Member))
            {
                foreach (Hour possibleStart in chairFreeSpan.FirstHoursOfDays)
                {
                    if (
                        _isPossibleStartForCaseWorker(possibleStart, chair)
                        && _isPossibleStartForCaseWorker(possibleStart, rapporteur)
                        && _isPossibleStartForCaseWorker(possibleStart, secondWorker))
                    {
                        Add(possibleStart, new OPCase(appealCase, caseBoard));
                        return;
                    }
                }
            }

            throw new InvalidOperationException("Could not schedule OP.");
        }



        
        internal override void UpdateSchedule(Hour currentHour)
        {
            _checkIsValidUpdateHour(currentHour);

            if (_lastUpdateHour != null && StartHours.Min() != null && currentHour > StartHours.Min())
                throw new InvalidOperationException($"Update out of sequence. Expected <= {StartHours.Min()} but got {currentHour}");

            _lastUpdateHour = currentHour;
            _processStartingCases(currentHour);
            _removeEntries(_memberSchedule, _memberSchedule.Keys.Where(x => x <= currentHour).ToList());
            _removeEntries(_blockedSchedule, _blockedSchedule.Keys.Where(x => x <= currentHour).ToList());
            _processFinishedCases(currentHour);
        }
        #endregion





        #region private methods       
        private void _recordForAllMembers(Hour startHour, Hour endHour, WorkCase workCase)
        {
            _recordForMember(startHour, endHour, workCase, workCase.Chair);
            _recordForMember(startHour, endHour, workCase, workCase.Rapporteur);
            _recordForMember(startHour, endHour, workCase, workCase.SecondWorker);
        }
        
        private void _recordForMember(
            Hour startHour, 
            Hour endHour, 
            WorkCase WorkCase, 
            CaseWorker worker)
        {
            Hour preparationStart = startHour.SubtractHours(worker.HoursOPPreparation);
            SimulationTimeSpan opSpan = new SimulationTimeSpan(preparationStart, endHour);

            foreach (Hour hour in opSpan)
            {
                if (_blockedSchedule.ContainsKey(hour) && _blockedSchedule[hour].Contains(worker.Member))
                    throw new InvalidOperationException(
                        string.Format("OPSchedule2.Add: attempt to schedule but member is blocked for {0}", hour));

                _addOPScheduleForWorker(hour, WorkCase, worker);
            }
        }
       

        private void _addOPScheduleForWorker(Hour hour, WorkCase WorkCase, CaseWorker worker)
        {
            if (_memberSchedule.ContainsKey(hour))
            {
                if (_memberSchedule[hour].ContainsKey(worker.Member))
                    throw new InvalidOperationException(
                        $"OPSchedule2._testAddOPDataScheduleForMember: member has already present for {hour}");

                _memberSchedule[hour][worker.Member] = WorkCase;
            }
            else
            {
                _memberSchedule[hour] = new Dictionary<Member, WorkCase>
                {
                    [worker.Member] = WorkCase
                };
            }
        }


        private void _blockSpanForAllMembers(Hour endHour, WorkCase WorkCase)
        {
            _blockSpanForOneMember(endHour, WorkCase.Chair);
            _blockSpanForOneMember(endHour, WorkCase.Rapporteur);
            _blockSpanForOneMember(endHour, WorkCase.SecondWorker);
        }
        
        private void _blockSpanForOneMember(Hour currentHour, CaseWorker worker)
        {
            if (_minimumDaysBetweenOP == 0)
                return;

            Hour blockStart = currentHour.Next();
            Hour blockEnd = blockStart.NextFirstHourOfDay().AddDays(_minimumDaysBetweenOP).Previous();
            SimulationTimeSpan blockSpan = new SimulationTimeSpan(blockStart, blockEnd);

            foreach (Hour hour in blockSpan)
            {
                _blockHourForMember(worker.Member, hour);
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
                if (HasOPWork(hour, worker.Member) || _isBlocked(hour, worker.Member))
                    return false;
            }

            return true;
        }

        private bool _isBlocked(Hour hour, Member member)
        {
            if (_blockedSchedule.ContainsKey(hour)
                && _blockedSchedule[hour].Contains(member))
                return true;

            return false;
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
            _startedCases.Clear();
            if (_startHours.ContainsKey(currentHour))
            {
                foreach (var x in _startHours[currentHour])
                {
                    _startedCases.Add(x);
                }
                _scheduleEndHours(currentHour);
                _startHours.Remove(currentHour);
            }
        }       

        private void _processFinishedCases(Hour currentHour)
        {
            _finishedCases.Clear();
            if (_endHours.ContainsKey(currentHour))
            {
                _finishedCases = _endHours[currentHour];
                _endHours.Remove(currentHour);
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
            toRemove.ForEach(x => dictionary.Remove(x));
        }

        private void _checkIsValidUpdateHour(Hour currentHour)
        {
            if (currentHour == null)
                throw new InvalidOperationException("Attempt to update at currentHour == null.");

            if (_lastUpdateHour == null)
                return;

            if (currentHour <= _lastUpdateHour)
                throw new InvalidOperationException($"Attempt to update out of sequence. Expected > {_lastUpdateHour} but got {currentHour}");

            Hour firstStartHour = StartHours.Min();
            if (firstStartHour == null)
                return;

            if (currentHour > firstStartHour)
                throw new InvalidOperationException($"Expected <= first start time: {firstStartHour} but got {currentHour}");

            Hour firstEndHour = _endHours.Keys.Min();
            if (firstEndHour == null)
                return;

            if (currentHour > firstEndHour)
                throw new InvalidOperationException($"Expected <= first end time: {firstEndHour} but got {currentHour}");

        }
        #endregion
    }
}