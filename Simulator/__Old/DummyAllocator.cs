using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldSim
{
    internal static class DummyAllocator
    {
        internal static CaseBoard GetAllocation (Member boardChair, IEnumerable<Member> technicals, IEnumerable<Member> legals)
        {
            return new CaseBoard(boardChair, technicals.ElementAt(0), legals.ElementAt(0));
        }

    }
}
