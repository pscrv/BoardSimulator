using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    internal class OPActivities
    {
        internal List<AllocatedCase> StartedCases;
        internal List<AllocatedCase> FinishedCases;


        internal OPActivities()
        {
            StartedCases = new List<AllocatedCase>();
            FinishedCases = new List<AllocatedCase>();
        }

        internal void Reset()
        {
            StartedCases.Clear();
            FinishedCases.Clear();
        }
    }
}
