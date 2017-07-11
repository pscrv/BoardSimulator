using System.Collections;
using System.Collections.Generic;

namespace OldSim
{
    internal class Engine
    {
        #region private fields
        private Board _board;
        private SimulationLog _log;
        private int _hoursToSimulate;
        #endregion


        #region internal properties
        internal SimulationLog Log { get { return _log; } }
        #endregion


        #region constructors
        internal Engine(Board board, IEnumerable<AppealCase> initialCases, int hours)
        {
            _board = board;
            _hoursToSimulate = hours;
            _log = new SimulationLog();

            foreach (AppealCase appealCase in initialCases)
            {
                _board.EnqueueNewCase(appealCase);
            }

        }
        #endregion


        #region internal methods
        internal void Run()
        {
            _initialiseRun();

            while (SimulationTime.Current.Value < _hoursToSimulate)
            {
                _board.DoAndLogWork(_log);
                SimulationTime.Increment();
            }
        }
        #endregion



        #region private methods
        private void _initialiseRun()
        {
            SimulationTime.Reset();
            _log = new SimulationLog();
        }
        #endregion
    }
}
