using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class SimulationTime
    {
        #region properties
        public int Hour { get; private set; }
        #endregion

        #region constructors
        public SimulationTime()
        {
            Hour = 0;
        }
        #endregion

        #region public methods
        public void Increment()
        {
            Hour++;
        }
        #endregion
    }
}
