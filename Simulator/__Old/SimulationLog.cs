using System;
using System.Collections.Generic;

namespace OldSim
{
    internal class SimulationLog
    {
        //#region private fields
        //private Dictionary<Hour, Dictionary<Member, WorkReport>> _log;
        //#endregion


        //#region constructors
        //internal SimulationLog()
        //{
        //    _log = new Dictionary<Hour, Dictionary<Member, WorkReport>>();
        //}
        //#endregion


        //#region internal methods
        //internal void Add(Member member, WorkReport log)
        //{
        //    Hour now = SimulationTime.Current;

        //    if (!_log.ContainsKey(now))
        //    {
        //        _log[now] = new Dictionary<Member, WorkReport>();
        //    }

        //    if (_log[now].ContainsKey(member))
        //        throw new InvalidOperationException("member has already logged work for the current hour.");

        //    _log[SimulationTime.Current][member] = log;
        //}
        //#endregion


        //#region public methods
        //public Dictionary<Member, WorkReport> GetForHour(Hour h)
        //{
        //    return _log[h];
        //}
        //#endregion
    }
}