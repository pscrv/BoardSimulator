using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Tests
{
    [TestClass()]
    public class MemberTests
    {
        Member member;
        

        [TestInitialize]
        public void Initialise()
        {
            member = new Member();
        }

        [TestMethod()]
        public void Enqueue()
        {
            int howmany = 4;

            AppealCase workingCase;
            for (int i = 0; i < howmany; i++)
            {
                workingCase = new AppealCase();
                workingCase.AdvanceState();
                member.EnqueueChairWork(workingCase);

                workingCase = new AppealCase();
                workingCase.AdvanceState();
                member.EnqueueChairWork(workingCase);

                workingCase = new AppealCase();
                workingCase.AdvanceState();
                member.EnqueueOtherWork(workingCase);

                workingCase = new AppealCase();
                workingCase.AdvanceState();
                member.EnqueueRapporteurWork(workingCase);
            }

            Assert.AreEqual(howmany * 4, member.CaseCount);
        }
    }
}