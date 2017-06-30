using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Board
    {
        #region private fields
        private Member _chair;
        private List<Member> _technical;
        private List<Member> _legal;

        private CaseQueue _incomingCases;
        private CaseQueue _activeCases;
        #endregion


        #region internal properties
        internal HourlyBoardLog Log { get; private set; }
        internal int IncomingCaseCount { get { return _incomingCases.Count; } }
        #endregion


        #region constructors
        internal Board(Member chair, List<Member> technical, List<Member> legal)
        {
            _chair = chair;
            _technical = technical;
            _legal = legal;

            _incomingCases = new CaseQueue();
            _activeCases = new CaseQueue();

            Log = null;
        }

        internal void EnqueueNewCase(AppealCase appealCase)
        {
            _incomingCases.Enqueue(appealCase);
        }

        internal void DoWork()
        {
            // do we need two queues?
            // can we tell from the sort of work
            // all we need to know?

            _processCases(_activeCases);
            _processCases(_incomingCases);
            Log = new HourlyBoardLog();

            _chair.DoWork();
            foreach (Member tm in _technical)
            {
                tm.DoWork();
            }
            foreach (Member lm in _legal)
            {
                lm.DoWork();
            }


            throw new NotImplementedException("DoWork not fully implemented.");
        }
        #endregion



        #region private methods
        private void _processCases(CaseQueue cases)
        {
            while (cases.Count > 0)
            {
                AllocatedCase allocated = _allocate(cases.Dequeue());
                Member firstWorker = allocated.NextSummonsWorker();
                firstWorker.EnqueueRapporteurWork(allocated.AppealCase);
            }
        }

        private AllocatedCase _allocate(AppealCase appealCase)
        {
            // TODO: make a proper allocation
            return new AllocatedCase(appealCase, _chair, _technical[0], _legal[0]);
        }
        #endregion
    }
}
