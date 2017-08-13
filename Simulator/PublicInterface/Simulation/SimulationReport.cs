using System.Collections.ObjectModel;

namespace Simulator
{
    public class SimulationReport
    {
        public readonly ReadOnlyCollection<CompletedCaseReport> FinishedCases;
        public readonly HourlyReports HourlyReports;
        private HourlyReports _reports;

        public SimulationReport(ReadOnlyCollection<CompletedCaseReport> finished, HourlyReports reports)
        {
            FinishedCases = finished;
            _reports = reports;
        }
    }
}