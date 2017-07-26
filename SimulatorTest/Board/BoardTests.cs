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
        CaseBoard caseBoard1;
        CaseBoard caseBoard2;

        AppealCase appealCase1 = new AppealCase();
        AppealCase appealCase2 = new AppealCase();
        AllocatedCase allocatedCase1;
        AllocatedCase allocatedCase2;

        MemberParameters memberParameters = new MemberParameters(2, 1, 2);
        MemberParameterCollection parameterCollection;
        
        Member chair;
        List<Member> technicals;
        List<Member> legals;


        [TestInitialize]
        public void Initialise()
        {
            SimulationTime.Reset();
            WorkQueues.ClearAllQueues();

            parameterCollection = new MemberParameterCollection(memberParameters, memberParameters, memberParameters);

            chair = new Member(parameterCollection);
            technicals = new List<Member> { new Member(parameterCollection) };
            legals = new List<Member> { new Member(parameterCollection) };

            board = new Board(chair, ChairType.Technical, technicals, legals);
            caseBoard1 = new CaseBoard(chair, technicals[0], legals[0]);
            caseBoard2 = new CaseBoard(chair, technicals[0], legals[0]);
            allocatedCase1 = new AllocatedCase(appealCase1, caseBoard1);
            allocatedCase2 = new AllocatedCase(appealCase2, caseBoard2);
        }



        [TestMethod()]
        public void Work_oneCase()
        {
            WorkQueues.Incoming.Enqueue(allocatedCase1);
            while (allocatedCase1.Record.ChairDecision.Finish == null)
            {
                if (allocatedCase1.Stage == CaseStage.OP && allocatedCase1.Record.OP.Enqueue != null)
                {
                    allocatedCase1.Record.SetOPStart();
                    allocatedCase1.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase1);
                }

                board.DoWork();
                SimulationTime.Increment();
            }

            _case1Assertions();

        }


        [TestMethod()]
        public void Work_twoCases()
        {
            WorkQueues.Incoming.Enqueue(allocatedCase1);
            WorkQueues.Incoming.Enqueue(allocatedCase2);

            while (allocatedCase2.Record.ChairDecision.Finish == null)
            {
                if (allocatedCase1.Stage == CaseStage.OP && allocatedCase1.Record.OP.Enqueue != null)
                {
                    allocatedCase1.Record.SetOPStart();
                    allocatedCase1.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase1);
                }

                if (allocatedCase2.Stage == CaseStage.OP && allocatedCase2.Record.OP.Enqueue != null)
                {
                    allocatedCase2.Record.SetOPStart();
                    allocatedCase2.Record.SetOPFinished();
                    WorkQueues.Circulation.Enqueue(allocatedCase2);
                }

                board.DoWork();
                SimulationTime.Increment();
            }

            _case1Assertions();
            _case2Assertions();

        }





        private void _case1Assertions()
        {
            Assert.AreEqual(0, allocatedCase1.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase1.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(0, allocatedCase1.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(1, allocatedCase1.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(2, allocatedCase1.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase1.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase1.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase1.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase1.Record.ChairSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase1.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase1.Record.OP.Enqueue.Value);

            Assert.AreEqual(7, allocatedCase1.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(7, allocatedCase1.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(8, allocatedCase1.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(9, allocatedCase1.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(9, allocatedCase1.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(10, allocatedCase1.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(11, allocatedCase1.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(11, allocatedCase1.Record.ChairDecision.Start.Value);
            Assert.AreEqual(12, allocatedCase1.Record.ChairDecision.Finish.Value);
        }

        private void _case2Assertions()
        {
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