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
    public class CaseLoggingTests
    {
        Member chair = new Member();
        Member rapporteur = new Member();
        Member other = new Member();
        AppealCase appealCase = new AppealCase();

        AllocatedCase allocatedCase;
        CaseBoard caseBoard; 

         [TestInitialize]
        public void Initialise()
        {
            caseBoard = new CaseBoard(chair, rapporteur, other);
            allocatedCase = new AllocatedCase(appealCase, caseBoard);
        }


        [TestMethod()]
        public void Enqueue()
        {
            allocatedCase.EnqueueForWork();

            Hour hour = new Hour(0);
            Assert.AreEqual(hour, allocatedCase.Record.Creation);
            Assert.AreEqual(hour, allocatedCase.Record.Allocation);
            Assert.AreEqual(hour, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.IsNull(allocatedCase.Record.OtherMemberSummons.Enqueue);
            Assert.IsNull(allocatedCase.Record.ChairSummons.Enqueue);
        }

        [TestMethod()]
        public void RapporteurWork()
        {
            allocatedCase.EnqueueForWork();
            allocatedCase.Board.Rapporteur.Member.Work();

            SimulationTime.Increment();
            allocatedCase.Board.Rapporteur.Member.Work();


            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Start);
            Assert.AreEqual(hour1, allocatedCase.Record.RapporteurSummons.Finish);
        }
    }
}