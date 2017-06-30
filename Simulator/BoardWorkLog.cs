using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class BoardWorkLog
    {
        #region private fields
        private Dictionary<Hour, HourlyBoardLog> _log;
        #endregion

        #region internal methods
        internal void Add(HourlyBoardLog log)
        {
            _log[SimulationTime.Current] = log;
        }
        #endregion
    }
}