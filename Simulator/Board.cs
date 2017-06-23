using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Board
    {
        #region private fields
        private Chair _chair;
        private List<TechnicalMember> _technicalMembers;
        private List<LegalMember> _legalMembers;
        #endregion

        #region constructors
        internal Board (Chair ch, List<TechnicalMember> tech, List<LegalMember> legal)
        {
            _chair = ch;
            _technicalMembers = tech;
            _legalMembers = legal;
        }
        #endregion


        #region private methods
        private IEnumerable<ChairWorker> Members()
        {
            yield return _chair;
            foreach (ChairWorker bm in _technicalMembers)
                yield return bm;
            foreach (ChairWorker bm in _legalMembers)
                yield return bm;
        }
        #endregion


        #region internal methods
        internal void DoWork(Hour hour)
        {
            _chair.DoWork(hour);
            foreach (TechnicalMember tm in _technicalMembers)
                tm.DoWork(hour);
            foreach (LegalMember lm in _legalMembers)
                lm.DoWork(hour);
        }

        internal void DM(Hour hour)
        {
            foreach (ChairWorker member in Members())
                member.DoWork(hour);
        }
        #endregion
    }
}