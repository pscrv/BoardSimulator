namespace Simulator
{
    internal class HourlyMemberLog
    {
        internal readonly BoardMember Member;
        internal readonly Hour Hour;
        internal readonly Work WorkDone;

        internal HourlyMemberLog(BoardMember bm, Hour h, Work work)
        {
            Member = bm;
            Hour = h;
            WorkDone = work;
        }
    }


    internal class OPLog : HourlyMemberLog
    {
        public OPLog(BoardMember bm, Hour h, OPWork work) 
            : base(bm, h, work) { }
    }
}