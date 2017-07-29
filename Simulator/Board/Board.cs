﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class Board
    {
        #region fields and properties
        private Member _chair;
        private ChairType _chairType;
        private List<Member> _technicals;
        private List<Member> _legals;

        private BoardQueue _boardQueue;
        private IncomingCaseQueue _incoming;
        private CirculationQueue _circulation;
        private OPSchedule _opSchedule;

        private Dictionary<Member, int> _allocationCount;

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
        #endregion



        #region construction
        internal Board(
            Member chair, 
            ChairType chairType, 
            List<Member> technicals, 
            List<Member> legals,
            BoardQueue boardQueues,
            IncomingCaseQueue incoming,
            CirculationQueue circulation,
            OPSchedule opSchedule)
        {
            _chair = chair;
            _chairType = chairType;
            _technicals = technicals;
            _legals = legals;

            _boardQueue = boardQueues;
            _incoming = incoming;
            _circulation = circulation;
            _opSchedule = opSchedule;

            _allocationCount = new Dictionary<Member, int>();
            foreach (Member member in _members)
            {
                _allocationCount[member] = 0;
            }
        }
        #endregion



        internal void DoWork(Hour currentHour)
        {
            _incoming.EnqueueForNextStage(currentHour);
            _opSchedule.EnqueueFinishedCasesForDecision(currentHour);
            _circulation.EnqueueForNextStage(currentHour);            

            foreach (Member member in _members)
            {
                member.Work(currentHour);
            }
        }


        internal void ProcessNewCase(AppealCase appealCase, Hour currentHour)
        {
            AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
            _incoming.Enqueue(currentHour, allocatedCase);
        }

        private AllocatedCase _allocateCase(AppealCase appealCase, Hour currentHour)
        {
            // TODO: make this choose (sometimes? when?) a different chair
            // than the board chair

            // TODO: do better than just counting allocations

            Member chair = _chair;
            Member rapporteur = _getMemberWithFewestAllocations(_technicals);
            Member other = _getMemberWithFewestAllocations(_legals);

            _allocationCount[chair]++;
            _allocationCount[rapporteur]++;
            _allocationCount[other]++;

            CaseBoard board = new CaseBoard(chair, rapporteur, other, _boardQueue);

            return new AllocatedCase(appealCase, board, currentHour, _opSchedule);
        }

        private Member _getMemberWithFewestAllocations(List<Member> members)
        {
            return members.Aggregate(
                (currentMin, m) => _allocationCount[m] < _allocationCount[currentMin] ? m : currentMin );            
        }
    }
}