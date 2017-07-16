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
        public void Work_oneCase()
        {
            WorkQueues.Incoming.Enqueue(allocatedCase);
            for (int i = 0; i < 14; i++)
            {
                if (i == 7)
                {
                    allocatedCase.Record.SetOPStart();
                    allocatedCase.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase);
                }

                board.DoWork();
                SimulationTime.Increment();
            }            


            Assert.AreEqual(0, allocatedCase.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(0, allocatedCase.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(1, allocatedCase.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(2, allocatedCase.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase.Record.ChairSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase.Record.OP.Enqueue.Value);

            Assert.AreEqual(7, allocatedCase.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(7, allocatedCase.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(8, allocatedCase.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(9, allocatedCase.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(9, allocatedCase.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(10, allocatedCase.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(11, allocatedCase.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(11, allocatedCase.Record.ChairDecision.Start.Value);
            Assert.AreEqual(12, allocatedCase.Record.ChairDecision.Finish.Value);

        }


        [TestMethod()]
        public void Work_twoCases()
        {
            CaseBoard caseBoard2 = new CaseBoard(chair, technicals[0], legals[0]);
            AllocatedCase allocatedCase2 = new AllocatedCase(new AppealCase(), caseBoard2);

            WorkQueues.Incoming.Enqueue(allocatedCase);
            WorkQueues.Incoming.Enqueue(allocatedCase2);

            for (int i = 0; i < 20; i++)
            {
                if (i == 7)
                {
                    allocatedCase.Record.SetOPStart();
                    allocatedCase.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase);
                }

                if (i == 9)
                {
                    allocatedCase2.Record.SetOPStart();
                    allocatedCase2.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase2);
                }

                board.DoWork();
                SimulationTime.Increment();
            }


            Assert.AreEqual(0, allocatedCase.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(0, allocatedCase.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(1, allocatedCase.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(2, allocatedCase.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase.Record.ChairSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase.Record.OP.Enqueue.Value);

            Assert.AreEqual(7, allocatedCase.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(7, allocatedCase.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(8, allocatedCase.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(9, allocatedCase.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(9, allocatedCase.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(10, allocatedCase.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(11, allocatedCase.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(11, allocatedCase.Record.ChairDecision.Start.Value);
            Assert.AreEqual(12, allocatedCase.Record.ChairDecision.Finish.Value);


            Assert.AreEqual(0, allocatedCase2.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase2.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase2.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase2.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase2.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase2.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase2.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase2.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(6, allocatedCase2.Record.ChairSummons.Start.Value);
            Assert.AreEqual(7, allocatedCase2.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(8, allocatedCase2.Record.OP.Enqueue.Value);

            Assert.AreEqual(9, allocatedCase2.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(9, allocatedCase2.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(10, allocatedCase2.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(11, allocatedCase2.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(11, allocatedCase2.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(12, allocatedCase2.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(13, allocatedCase2.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(13, allocatedCase2.Record.ChairDecision.Start.Value);
            Assert.AreEqual(14, allocatedCase2.Record.ChairDecision.Finish.Value);

        }
    }
}