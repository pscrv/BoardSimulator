using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimulatorB
{
    internal class SimulationReport
    {
        internal readonly ReadOnlyCollection<CompletedCaseReport> FinishedCases;
        internal readonly HourlyReports HourlyReports;

        public SimulationReport(IEnumerable<CompletedCaseReport> finished, HourlyReports reports)
        {
            FinishedCases = finished.ToList().AsReadOnly();
            HourlyReports = reports;
        }
    }
}