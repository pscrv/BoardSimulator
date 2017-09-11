using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal abstract class OPSchedule
    {
        internal abstract int Count { get; }
        internal abstract List<Hour> StartHours { get; }
        internal abstract List<AllocatedCase> RunningCases { get; }
        internal abstract List<AllocatedCase> StartedCases { get; }
        internal abstract List<AllocatedCase> FinishedCases { get; }

        internal abstract void Add(Hour hour, AllocatedCase allocatedCase);
        internal abstract bool HasOPWork(Hour hour, Member member);
        internal abstract AllocatedCase GetOPWork(Hour hour, Member member);
        internal abstract void Schedule(Hour currentHour, AllocatedCase allocatedCase);
        internal abstract void UpdateSchedule(Hour currentHour);
        internal abstract bool IsBlocked(Hour hour, Member member);
    }


    internal class SimpleOPScheduler : OPSchedule
    {
        #region fields and properties
        private int _minimumDaysBetweenOP;
        private Dictionary<Hour, Dictionary<Member, AllocatedCase>> _memberSchedule;
        private Dictionary<Hour, HashSet<Member>> _blockedSchedule;
        private Dictionary<Hour, List<AllocatedCase>> _startHours;
        private Dictionary<Hour, List<AllocatedCase>> _endHours;
        
        Hour _lastUpdateHour;
        private List<AllocatedCase> _startedCases;
        private List<AllocatedCase> _finishedCases;
        internal override List<AllocatedCase> StartedCases { get => _startedCases; }
        internal override List<AllocatedCase> FinishedCases { get => _finishedCases; }
        #endregion



        #region construction
        internal SimpleOPScheduler(int minimumDaysBetweenOP)
        {
            _minimumDaysBetweenOP = minimumDaysBetweenOP;
            
            _memberSchedule = new Dictionary<Hour, Dictionary<Member, AllocatedCase>>();
            _blockedSchedule = new Dictionary<Hour, HashSet<Member>>();
            _startHours = new Dictionary<Hour, List<AllocatedCase>>();
            _endHours = new Dictionary<Hour, List<AllocatedCase>>();

            _lastUpdateHour = null;
            _startedCases = new List<AllocatedCase>();
            _finishedCases = new List<AllocatedCase>();
        }

        internal SimpleOPScheduler()
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
        

        internal override bool IsBlocked(Hour hour, Member member)
        {
            if (_blockedSchedule.ContainsKey(hour)
                && _blockedSchedule[hour].Contains(member))
                return true;

            return false;
        }

        internal override int Count => _startHours.Sum(x => x.Value.Count);

        internal override List<Hour> StartHours => _startHours.Keys.ToList();


        internal override List<AllocatedCase> RunningCases
        {
            get => _endHours.Values.Aggregate(
                new List<AllocatedCase>(), 
                (a, b) => a.Concat(b).ToList());
        }

        internal override void Schedule(Hour currentHour, AllocatedCase allocatedCase)
        {
            Hour firstPossibleHour = currentHour.NextFirstHourOfDay().AddMonths(TimeParameters.OPMinimumMonthNotice);

            CaseWorker chair = allocatedCase.GetCaseWorkerByRole(WorkerRole.Chair);
            CaseWorker rapporteur = allocatedCase.GetCaseWorkerByRole(WorkerRole.Rapporteur);
            CaseWorker otherWorker = allocatedCase.GetCaseWorkerByRole(WorkerRole.OtherMember);
            
            foreach (
                SimulationTimeSpan chairFreeSpan in 
                    _getFreeIntervals(firstPossibleHour, chair.Member))           
            {
                foreach (Hour possibleStart in chairFreeSpan.FirstHoursOfDays)
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


        internal override void UpdateSchedule(Hour currentHour)
        {
            _lastUpdateHour = currentHour;
            _processStartingCases(currentHour);
            _removeEntries(_memberSchedule, _memberSchedule.Keys.Where(x => x <= currentHour).ToList());
            _removeEntries(_blockedSchedule, _blockedSchedule.Keys.Where(x => x <= currentHour).ToList());
            _processFinishedCases(currentHour);
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
            Hour blockEnd = blockStart.NextFirstHourOfDay().AddDays(_minimumDaysBetweenOP).Previous();
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
        #endregion
    }
}