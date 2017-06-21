using System;
using System.Collections.Generic;

namespace Simulator
{
    //internal class OPSchedule
    //{
    //    private static int __minimumLeadTime = 20;
    //    private static int __opDuration = 6;

    //    private Timespan _span;
    //    private Dictionary<BoardMember, List<Hour>> _opStartTimes;
    //    private int _hoursForOP;

    //    internal OPSchedule(List<BoardMember> members, Timespan span)
    //    {
    //        _span = span;
    //        _opStartTimes = new Dictionary<BoardMember, List<Hour>>(members.Count);
    //        foreach (BoardMember member in members)
    //        {
    //            _opStartTimes[member] = new List<Hour>(span.Length);
    //        }
    //    }


    //    internal bool TryScheduleOP(Hour hour, BoardMember member)
    //    {
    //        if (_isPossibleHour(hour, member))
    //        {
    //            _opStartTimes[member].Add(hour);
    //            return true;
    //        }
    //        return false;
    //    }

    //    private bool _isPossibleHour(Hour hour, BoardMember member)
    //    {
    //        Hour nextOPStartHour = _nextOPStartHour(hour, member);
    //        Hour previousOPStartHour = _previousOPStartHour(hour, member);

    //        bool enoughLeadTime = (hour.Value - previousOPStartHour.Value - __opDuration) >= __minimumLeadTime;
    //        bool enoughFollowingTime = (nextOPStartHour.Value - hour.Value - __opDuration) >= __minimumLeadTime;

    //        return enoughLeadTime && enoughFollowingTime;
    //    }

    //    private Hour _nextOPStartHour(Hour hour, BoardMember member)
    //    {
    //        Hour nextOPStartHour = _opStartTimes[member].Find(x => x.Value >= hour.Value);
    //        if (nextOPStartHour == null)
    //            nextOPStartHour = new Hour(_span.Length);
    //        return nextOPStartHour;
    //    }

    //    private Hour _previousOPStartHour(Hour hour, BoardMember member)
    //    {
    //        Hour previousOPStartHour = _opStartTimes[member].FindLast(x => x.Value < hour.Value);
    //        if (previousOPStartHour == null)
    //            previousOPStartHour = new Hour(0);
    //        return previousOPStartHour;
    //    }




    //    //internal bool HasOP(Hour hour, BoardMember member)
    //    //{
    //    //    Hour nextOPStartHour = _nextOPStartHour(hour, member);
    //    //    if (nextOPStartHour.Value - hour.Value <= member)
    //    //        return true;
    //    //    Hour previousOPStartHour = _previousOPStartHour(hour, member);
    //    //    if (hour.Value - previousOPStartHour.Value - __opDuration <= 0)
    //    //        return true;
    //    //    return false;
    //    //}
    //}
}