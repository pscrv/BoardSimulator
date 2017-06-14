using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Board
    {
        private Chair _chair;
        private List<TechnicalMember> _technicalMembers;
        private List<LegalMember> _legalMembers;


        internal Board()
        {
            _chair = new Chair();
            _technicalMembers = new List<TechnicalMember> { new TechnicalMember() };
            _legalMembers = new List<LegalMember> { new LegalMember(), new LegalMember() };
        }

        internal Board(Chair ch, List<TechnicalMember> tms, List<LegalMember> lms)
        {
            _chair = ch;
            _technicalMembers = new List<TechnicalMember>();
            _legalMembers = new List<LegalMember>();

            foreach (TechnicalMember tm in tms)
                if (tm != (BoardMember)ch)
                    _technicalMembers.Add (tm);
            foreach (LegalMember lm in lms)
                if (lm != (BoardMember)ch)
                    _legalMembers.Add (lm);

            if (_chair == null)
                throw new ArgumentNullException("Board cannot have null Chair.");
            if (_technicalMembers.Count == 0)
                throw new ArgumentException("Board must have at least one TechnicalMember.");
            if (_legalMembers.Count == 0)
                throw new ArgumentException("Board must have at least one LegalMember.");

        }


        internal IEnumerable<BoardMember> Members
        {
            get
            {
                yield return _chair;
                foreach (BoardMember m in _technicalMembers)
                    yield return m;
                foreach (BoardMember m in _legalMembers)
                    yield return m;
            }
        }


        internal BoardState DoOneHourOfWork(BoardState boardState)
        {
            foreach (BoardMember bm in Members)
                boardState.UpdateMemberState (bm.DoOneHourOfWork());
            return boardState;
        }
    }
}