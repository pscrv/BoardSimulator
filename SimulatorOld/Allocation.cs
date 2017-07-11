using System;

namespace SimulatorOld
{
    internal class Allocation
    {
        #region private fields
        private WorkerQueue _allocatedBoard;
        #endregion


        #region constructors
        internal Allocation(Member chair, Member rapporteur, Member other)
        {
            _allocatedBoard = new WorkerQueue(chair, rapporteur, other);
        }
        #endregion


        #region internal methods
        internal Member NextSummonsWorker()
        {
            return _allocatedBoard.DequeueSummonsWorker();
        }

        internal Member Enqueue(AppealCase appealCase)
        {
            switch (appealCase.Stage)
            {
                case AppealCaseState.Stage.New:
                    appealCase.AdvanceState();
                    break;
                case AppealCaseState.Stage.SummonsEnqueued:
                    appealCase.AdvanceState();
                    break;
                case AppealCaseState.Stage.SummonsStarted:
                    break;
                case AppealCaseState.Stage.SummonsFinished:
                    break;
                case AppealCaseState.Stage.OPPending:
                    break;
                case AppealCaseState.Stage.OPFinsished:
                    break;
                case AppealCaseState.Stage.DecisionEnqueued:
                    break;
                case AppealCaseState.Stage.DecisionStarted:
                    break;
                case AppealCaseState.Stage.DecisionFinished:
                    break;
                default:
                    break;
            }



            if (_allocatedBoard.SummonsWorkerCount == 0)
                return null;

            Member member = _allocatedBoard.DequeueSummonsWorker();
            switch (_allocatedBoard.SummonsWorkerCount)
            {
                case 2:
                    member.EnqueueRapporteurWork(appealCase);
                    break;
                case 1:
                    member.EnqueueOtherWork(appealCase);
                    break;
                case 0:
                    member.EnqueueChairWork(appealCase);
                    break;
                default:
                    throw new InvalidOperationException("Something is wrong with the number of summons workers.");
            }

            return member;
        }

        internal void NotifyOPSchedule(AppealCase ac, Hour opStartHour, Hour opEndHour)
        {
            foreach (Member member in _allocatedBoard)
                member.RegisterOP(ac, opStartHour, opEndHour);
        }
        #endregion
    }
}