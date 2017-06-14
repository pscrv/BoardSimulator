namespace Simulator
{
    internal class CaseLog
    {
        // Something like the following
        internal SimulationTime ArrivalTime { get; private set; }
        internal SimulationTime StartTime { get; private set; }
        internal SimulationTime SummonsOutTime { get; private set; }

        internal SimulationTime OPStartTime { get; private set; }
        internal SimulationTime OPEndTime { get; private set; }
        internal SimulationTime DecisionOutTime { get; private set; }
    }
}