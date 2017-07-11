using System;
using System.Collections.Generic;

namespace OldSim
{
    internal class Board
    {
        #region private fields
        private Member _chair;
        private List<Member> _technicals;
        private List<Member> _legals;

        private Dictionary<AppealCase, CaseBoard> _allocations;
        private List<AppealCase> _finishedWork;

        private Dictionary<AppealCase, AllocatedCase> _alls;

        #endregion


        #region internal properties
        internal IEnumerable<Member> Members
        {
            get
            {
                yield return _chair;
                foreach (Member m in _technicals) yield return m;
                foreach (Member m in _legals) yield return m;
            }
        }
        #endregion

        


        #region constructors
        internal Board(Member chair, List<Member> technical, List<Member> legal)
        {
            _chair = chair;
            _technicals = technical;
            _legals = legal;

            _allocations = new Dictionary<AppealCase, CaseBoard>();
            _alls = new Dictionary<AppealCase, AllocatedCase>();
        }
        #endregion



        #region internal methods
        internal void EnqueueNewCase(AppealCase appealCase)
        {
            if (_allocations.ContainsKey(appealCase))
                throw new InvalidOperationException("appealCase is already in the queue.");

            if (appealCase.Stage != AppealCaseState.Stage.New)
                throw new InvalidCastException("Cannot enqueue a case in stage" + appealCase.Stage + ".");

            CaseBoard allocation = DummyAllocator.GetAllocation(_chair, _technicals, _legals);
            _allocations[appealCase] = allocation;
            {

                //Member first = allocation.GetNextWorker();
                //if (first != null)
                //{
                //    first.Enqueue(appealCase);
                //}
                //else
                //{
                //    throw new InvalidOperationException("New allocation went wrong.");
                //}
            }

            {
                Member first = allocation.GetNextWorker();
                AllocatedCase allCase = new AllocatedCase(appealCase, allocation);
                _alls[appealCase] = allCase;
                if (first != null)
                {
                    first.Enqueue(allCase);
                }
                else
                {
                    throw new InvalidOperationException("New allocation went wrong.");
                }

            }


        }



        internal void DoAndLogWork(SimulationLog log)
        {
            foreach (Member member in Members)
                member.DoAndLogWork(log);
        }
        #endregion

    
    }
}
