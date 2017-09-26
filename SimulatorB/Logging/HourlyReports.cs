using System;
using System.Collections.Generic;

namespace SimulatorB
{
    public class HourlyReports
    {
        #region fields and properties
        private readonly Dictionary<Hour, BoardReport> _reports = new Dictionary<Hour, BoardReport>();
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
    }
}