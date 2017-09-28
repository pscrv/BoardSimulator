using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimulatorB.PublicInterface
{
    public class SimulationReport
    {
        public readonly PublicCasesReportList FinishedCases;
        public readonly PublicHourlyReports HourlyReports;

        internal SimulationReport(IEnumerable<CompletedCaseReport> finished, HourlyReports reports)
        {
            FinishedCases = new PublicCasesReportList(
                finished.Select(x => new PublicCaseReport(x)).ToList());

            HourlyReports = reports.AsPublicHourlyReports();
        }
    }
}