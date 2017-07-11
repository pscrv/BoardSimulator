using System;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class BoardWorkLog
    {
        #region private fields
        private Dictionary<Hour, BoardLog> _log;
        #endregion


        #region constructors
        internal BoardWorkLog()
        {
            _log = new Dictionary<Hour, BoardLog>();
        }
        #endregion


        #region internal methods
        internal void Add(BoardLog log)
        {
            _log[SimulationTime.Current] = log;
        }

        internal BoardLog GetForHour(Hour h)
        {
            return _log[h];
        }
        #endregion
    }
}