namespace SimulatorB.PublicInterface
{
    public class PublicCaseReport
    {
        public readonly int AppealCaseID;
        public readonly int ChairID;
        public readonly int RapporteurID;
        public readonly int SecondMemberID;

        public readonly FinishedCaseLog Log;


        internal PublicCaseReport(CompletedCaseReport report)
        {
            AppealCaseID = report.AppealCase.ID;
            ChairID = report.CaseBoard.Chair.Member.ID;
            RapporteurID = report.CaseBoard.Rapporteur.Member.ID;
            SecondMemberID = report.CaseBoard.SecondWorker.Member.ID;
            Log = new FinishedCaseLog(report.Log);
        }

    }




}


