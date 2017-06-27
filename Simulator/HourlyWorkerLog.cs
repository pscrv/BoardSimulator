namespace Simulator
{
    internal enum WorkType { SummonsWork, DecisionWork, OPWork, NoWork }


    internal class HourlyworkerLog
    {
        internal readonly WorkType WorkDone;
        internal readonly Case workedCase;
        
        internal HourlyworkerLog(WorkType type, Case workedcase)
        {
            WorkDone = type;
            workedCase = workedcase;
        }
    }


    internal class OPLog : HourlyworkerLog
    {
        public OPLog(OPWork work)
            : base(WorkType.OPWork, null) { }
    }
}