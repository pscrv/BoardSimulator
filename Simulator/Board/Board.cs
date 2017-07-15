using System.Collections.Generic;

namespace Simulator
{
    internal class Board
    {
        #region fields and properties
        private Member _chair;
        private ChairType _chairType;
        private List<Member> _technicals;
        private List<Member> _legals;
        #endregion



        #region construction
        internal Board(Member chair, ChairType chairType, List<Member> technicals, List<Member> legals)
        {
            _chair = chair;
            _chairType = chairType;
            _technicals = technicals;
            _legals = legals;
        }
        #endregion



        internal void DoWork()
        {
            WorkQueues.Incoming.PassCasesToMembers();

            foreach (Member member in _members)
            {
                member.Work();
            }
        }



        private IEnumerable<Member> _members
        {
            get
            {
                yield return _chair;
                foreach (Member tm in _technicals)
                    yield return tm;
                foreach (Member lm in _legals)
                    yield return lm;                
                
            }
        }
    }
}