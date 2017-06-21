using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Board
    {
        private BoardMember _chair;
        private List<TechnicalMember> _technicalMembers;
        private List<LegalMember> _legalMembers;


        internal Board (BoardMember ch, List<TechnicalMember> tech, List<LegalMember> legal)
        {
            _chair = ch;
            _technicalMembers = tech;
            _legalMembers = legal;
        }

       
        private IEnumerable<BoardMember> Members()
        {
            yield return _chair;
            foreach (BoardMember bm in _technicalMembers)
                yield return bm;
            foreach (BoardMember bm in _legalMembers)
                yield return bm;
        }

    }
}