using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    class Allocator
    {
        private Dictionary<Member, int> _allocationCount;

        
        internal Allocator(IEnumerable<Member> _members)
        {
            _allocationCount = new Dictionary<Member, int>();
            foreach (Member member in _members)
            {
                _allocationCount[member] = 0;
            }
        }



        //TODO: allow choice of chairs
        internal CaseBoard GetAllocation(
            AppealCase appealCase,
            Member chair,
            IEnumerable<Member> possibleRapporteurs,
            IEnumerable<Member> possibleSecondMembers)
        {
            Member rapporteur;
            Member second;

            rapporteur = _getMemberWithFewestAllocations(
                possibleRapporteurs
                .Where(x => x != chair));
            second = _getMemberWithFewestAllocations(
                possibleSecondMembers
                .Where(x => x != chair && x != rapporteur));

            _allocationCount[chair]++;
            _allocationCount[rapporteur]++;
            _allocationCount[second]++;
            

            return new CaseBoard(
                new ChairWorker(chair),
                new RapporteurWorker(rapporteur),
                new SecondWorker(second)
                );
        }



        private Member _getMemberWithFewestAllocations(IEnumerable<Member> members)
        {
            return members.Aggregate(
                (currentMin, m) => _allocationCount[m] < _allocationCount[currentMin] ? m : currentMin);
        }
    }
}
