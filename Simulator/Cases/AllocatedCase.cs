using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class AllocatedCase
    {
        internal readonly AppealCase Case;
        internal readonly CaseBoard Board;
        internal readonly CaseRecord Record;


        internal CaseStage Stage
        {
            get
            {
                if (Record.RapporteurSummons.Finish == null
                    || Record.OtherMemberSummons.Finish == null
                    || Record.ChairSummons.Finish == null)
                    return CaseStage.Summons;

                if (Record.OP.Finish == null)
                    return CaseStage.OP;

                if (Record.RapporteurDecision.Finish == null
                    || Record.OtherMemberDecision.Finish == null
                    || Record.ChairDecision.Finish == null)
                    return CaseStage.Decision;

                return CaseStage.Finished;
            }
        }

        internal WorkType WorkType
        {
            get
            {
                switch (Stage)
                {
                    case CaseStage.Summons:
                        return WorkType.Summons;
                    case CaseStage.Decision:
                        return WorkType.Decision;

                    case CaseStage.OP:
                    case CaseStage.Finished:
                        break;
                }
                return WorkType.None;
            }
        }

        internal AllocatedCase(AppealCase ac, CaseBoard bd)
        {
            Case = ac;
            Board = bd;

            Record = new CaseRecord(ac);
            Record.SetAllocation();            
        }



        internal void RecordStartOfWork(CaseWorker caseWorker)
        {
            WorkerRole role = caseWorker.Role;

            switch (WorkType)
            {
                case WorkType.Summons:
                    Record.RapporteurSummons.SetStart();
                    break;
                case WorkType.Decision:
                    Record.RapporteurDecision.SetStart();
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.RecordStartOfWork: no summons or decision work to start.");
                    
            }
        }


        internal void RecordFinishedWork(CaseWorker caseWorker)
        {
            WorkerRole role = caseWorker.Role;

            switch (WorkType)
            {
                case WorkType.Summons:

                    switch (role)
                    {
                        case WorkerRole.Rapporteur:

                            if (true /* WorkIsFinsihed */)
                            {
                                Record.RapporteurSummons.SetFinish();
                                Board.EnqueueForOP();
                            }

                            break;
                        case WorkerRole.Chair:
                            break;
                        case WorkerRole.OtherMember:
                            break;
                        case WorkerRole.None:
                            throw new InvalidOperationException("AllocatedCase.RecordWork: caseWorker has no role for this case.");
                    }


                    break;
                case WorkType.Decision:
                    break;

                case WorkType.None:
                    throw new InvalidOperationException("AllocatedCase.Recordwork: there is no work to do.");
            }


        }



        internal void EnqueueForWork()
        {
            WorkerRole role = Board.EnqueueForNextWorker(this);
            Record.SetSummonsEnqueue(role);            
        }

        
               



    }
}