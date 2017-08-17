using Simulator;

namespace SimulatorUI
{
    public class SimulationReportViewModel
    {
        private int _finishedCaseCount;


        public int FinishedCaseCount { get { return _finishedCaseCount; } }



        public SimulationReportViewModel(SimulationReport report)
        {
            _finishedCaseCount = report?.FinishedCases.Count ?? 0;
        }

    }
}