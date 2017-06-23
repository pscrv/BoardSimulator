namespace Simulator
{
    internal class HourlyMemberLog
    {
        internal readonly ChairWorker Member;
        internal readonly Hour Hour;
        internal readonly Work WorkDone;

        internal HourlyMemberLog(ChairWorker bm, Hour h, Work work)
        {
            Member = bm;
            Hour = h;
            WorkDone = work;
        }
    }


    internal class OPLog : HourlyMemberLog
    {
        public OPLog(ChairWorker bm, Hour h, OPWork work) 
            : base(bm, h, work) { }
    }
}