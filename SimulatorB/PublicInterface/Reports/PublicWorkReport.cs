namespace SimulatorB
{

    public class PublicWorkReport
    {
        public readonly WorkType Type;
        public readonly bool Finished;
        
        internal PublicWorkReport(WorkReport report)
        {
            Type = report.Type;
            Finished = report.Finished;
        }        
    }
}
