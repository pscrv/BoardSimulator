using System; 

namespace Simulator
{
    internal class Member
    {
        #region temporary static
        private static int _HOURS_SUMMONS = 2;
        private static int _HOURS_OP_PREP = 1;
        private static int _HOURS_DECISION = 2;
        #endregion


        #region fields and properties
        private int _workCounter = 0;
        private AllocatedCase _currentCase { get { return WorkQueues.PeekForMember(this); } }
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

            WorkQueues.RegisterMember(this);
        }


        internal void Work()
        {
            if (_currentCase == null)
            {
                _logWork(); // no work
                return;
            }

            if (_workCounter == 0)
            {
                CaseWorker meAsCaseWorker = _currentCase.Board.GetMemberAsCaseWorker(this);
                
                _currentCase.RecordStartOfWork(meAsCaseWorker);
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
            switch (_currentCase.WorkType)
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
            _currentCase.RecordFinishedWork(_currentCase.Board.GetMemberAsCaseWorker(this));

            WorkQueues.EnqueueForCirculation(_currentCase);
            //_currentCase.EnqueueForWork();

            // the above line needs to put CurrentCase in a queue for the caseboard,
            // it should be enqueued for the next member on the next tick.
            // If we enqueue for the next member now, they may treat it this tick,
            // but the case has already taken up the whole tick.

            WorkQueues.DequeueForMember(this);
        }

        private void _logWork()
        {
            // TODO: log work
        }
    }
}
