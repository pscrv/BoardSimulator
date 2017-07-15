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
    public class BoardTests
    {
        Board board;
        CaseBoard caseBoard;

        AppealCase appealCase = new AppealCase();
        AllocatedCase allocatedCase;

        Member chair = new Member();
        List<Member> technicals = new List<Member> { new Member() };
        List<Member> legals = new List<Member> { new Member() };


        [TestInitialize]
        public void Initialise()
        {
            SimulationTime.Reset();
            WorkQueues.ClearAllQueues();

            board = new Board(chair, ChairType.Technical, technicals, legals);
            caseBoard = new CaseBoard(chair, technicals[0], legals[0]);
            allocatedCase = new AllocatedCase(appealCase, caseBoard);
        }



        [TestMethod()]
        public void Constructor()
        {
            board = new Board(chair, ChairType.Technical, technicals, legals);
        }

        [TestMethod()]
        public void DoWorkTest()
        {
            allocatedCase.EnqueueForWork();
            for (int i = 0; i < 7; i++)
            {
                WorkQueues.Circulation.PassCasesToMembers();
                board.DoWork();
                SimulationTime.Increment();
            }
        }
    }
}