using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Timeline
    {
        internal Timespan Span { get; private set; }
        private Dictionary<Hour, HourlyWorkLog> _hourlyLog;

        internal Timeline(Timespan span)
        {
            Span = span;
            _hourlyLog = new Dictionary<Hour, HourlyWorkLog>(span.Length);
            foreach (Hour hour in Span)
            {
                _hourlyLog[hour] = new UnfilledLog();
            }
        }

        internal void Add(Hour hour, HourlyWorkLog log)
        {
            if (_hourlyLog[hour].CanLog)
                _hourlyLog[hour] = log;
            else
                throw new InvalidOperationException("Attempt to overwrite a logged hour.");
        }
        

    }
}