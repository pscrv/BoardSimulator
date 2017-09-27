namespace SimulatorB
{
    internal class CompletedCaseReport
    {
        internal readonly AppealCase AppealCase;
        internal readonly CaseBoard CaseBoard;
        internal readonly CaseLog Log;


        internal CompletedCaseReport(AppealCase ac, CaseBoard cb, CaseLog log)
        {
            AppealCase = ac;
            CaseBoard = cb;
            Log = log;
        }
    }
}
