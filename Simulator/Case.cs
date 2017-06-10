using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class Case
    {
        public SimulationTime ArrivalTime { get; private set; }
        public SimulationTime StartTime { get; private set; }
        public SimulationTime SummonsOutTime { get; private set; }
        public SimulationTime OPStartTime { get; private set; }
        public SimulationTime OPEndTime { get; private set; }
        public SimulationTime DecisionOutTime { get; private set; }

        public CaseState State { get; }
    }
}
