using System;
using System.Collections.Generic;
using SimulatorB.PublicInterface;

namespace SimulatorB
{
    internal class HourlyReports
    {
        #region fields and properties
        private Dictionary<Hour, BoardReport> _reports = new Dictionary<Hour, BoardReport>();
        #endregion



        internal void Add(Hour hour, BoardReport report)
        {
            if (_reports.ContainsKey(hour))
                throw new InvalidOperationException("SimulationReports.Add: hour has already been recorded.");

            _reports.Add(hour, report);
        }


        internal BoardReport Read(Hour hour)
        {
            return _reports[hour];
        }


        internal PublicHourlyReports AsPublicHourlyReports()
        {
            Dictionary<int, PublicBoardReport> reports = new Dictionary<int, PublicBoardReport>();
            foreach (Hour hour in _reports.Keys)
            {
                reports[hour.Value] = _reports[hour].AsPublicBoardReport();
            }

            return new PublicHourlyReports(reports);
        }
    }
}