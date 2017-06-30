using System.Collections;
using System.Collections.Generic;

namespace Simulator
{
    internal class Engine
    {
        #region private fields
        private Board _board;
        private BoardWorkLog _log;
        private int _hoursToSimulate;
        #endregion

        #region constructors
        internal Engine(Board board, IEnumerable<AppealCase> initialCases, int hours)
        {
            _board = board;
            _log = new BoardWorkLog();

            foreach (AppealCase appealCase in initialCases)
            {
                _board.EnqueueNewCase(appealCase);
            }

            _hoursToSimulate = hours;
        }
        #endregion

        #region internal methods
        internal void Run()
        {
            _initialise();

            while (SimulationTime.Current.Value < _hoursToSimulate)
            {
                _board.DoWork();
                _log.Add(_board.Log);
            }
        }
        #endregion



        #region private methods
        private void _initialise()
        {
            SimulationTime.Reset();
        }
        #endregion
    }
}
