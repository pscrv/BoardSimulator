using System.Collections.Generic;

namespace SimulatorOld
{
    internal class OPList 
    {
        #region private fields
        private List<SimulationTimeSpan> _opTimes;
        private Dictionary<SimulationTimeSpan, AppealCase> _cases;
        #endregion




        #region internal properties
        internal AppealCase OPScheduledForCurrentHour
        {
            get
            {
                SimulationTimeSpan span = _opTimes.Find(x => x.ConatainsCurrent);
                if (span == null)
                    return null;
                return _cases[span];
            }
        }
        #endregion



        #region consctructors
        internal OPList()
        {
            _opTimes = new List<SimulationTimeSpan>();
            _cases = new Dictionary<SimulationTimeSpan, AppealCase>();
        }
        #endregion


        #region internal methods
        internal void Add(AppealCase ac, SimulationTimeSpan span)
        {
            _opTimes.Add(span);
            _cases[span] = ac;
        }
        #endregion
    }
}