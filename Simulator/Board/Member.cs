using System; 

namespace Simulator
{
    internal class Member
    {
        #region temporary static
        private static int _HOURS_SUMMONS = 2;
        private static int _HOURS_OP_PREP = 1;
        private static int _HOURS_DECISION = 3;
        #endregion


        #region fields and properties
        private int _workCounter = 0;
        private AllocatedCase CurrentCase { get { return CaseQueue.Peek(); } }

        internal readonly int HoursForSummons;
        internal readonly int HoursOPPrepration;
        internal readonly int HoursForDecision;
        
        internal MemberWorkQueue CaseQueue = new MemberWorkQueue();
        #endregion

        internal Member()
        {
            HoursForSummons = _HOURS_SUMMONS;
            HoursOPPrepration = _HOURS_OP_PREP;
            HoursForDecision = _HOURS_DECISION;
        }


        internal void Work()
        {
            if (CurrentCase == null)
            {
                _logWork(); // no work
                return;
            }

            if (_workCounter == 0)
            {
                CurrentCase.RecordStartOfWork(CurrentCase.Board.GetMemberAsCaseWorker(this));
                _setWorkCounter();
            }

            _workCounter--;
            _logWork();

            if (_workCounter == 0)
            {
                _finishCase();
            }
        }

        private void _setWorkCounter()
        {
            switch (CurrentCase.WorkType)
            {
                case WorkType.Summons:
                    _workCounter = HoursForSummons;
                    break;
                case WorkType.Decision:
                    _workCounter = HoursForDecision;
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("member.Work: no work to do on this case.");
            }
        }

        private void _finishCase()
        {
            CurrentCase.RecordFinishedWork(CurrentCase.Board.GetMemberAsCaseWorker(this));
            CaseQueue.Dequeue();
        }

        private void _logWork()
        {
            // TODO: log work
        }
    }
}
