using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{

    [TestClass()]
    public class BoardTests
    {
        Board board0;  // 0 day gap between OPs
        Board board2;  // 2 day gap between OPs
        Board boardL;  // Board with legally qualified chair

        AppealCase appealCase1 = new AppealCase();
        AppealCase appealCase2 = new AppealCase();
        AllocatedCase allocatedCase01;
        AllocatedCase allocatedCase02;
        AllocatedCase allocatedCase21;
        AllocatedCase allocatedCase22;

        MemberParameters memberParameters = new MemberParameters(2, 1, 2);
        MemberParameterCollection parameterCollection;
        
        Member chair;
        List<Member> technicals;
        List<Member> legals; 


        [TestInitialize]
        public void Initialise()
        {
            parameterCollection = new MemberParameterCollection(memberParameters, memberParameters, memberParameters);

            chair = new Member(parameterCollection);
            technicals = new List<Member> { new Member(parameterCollection) };
            legals = new List<Member> { new Member(parameterCollection) };

            board0 = Board.MakeTechnicalBoard(
                chair, 
                technicals, 
                legals,
                new Registrar(new SimpleOPScheduler()),
                new ChairChooser(chair));

            board2 = Board.MakeTechnicalBoard(
                chair,
                technicals,
                legals,
                new Registrar(new SimpleOPScheduler(2)),
                new ChairChooser(chair));

            boardL = Board.MakeLegalBoard(
                chair,
                new List<Member>
                {
                    new Member(parameterCollection),
                    new Member(parameterCollection)
                },
                legals,
                new Registrar(new SimpleOPScheduler()),
                new ChairChooser(chair));

            allocatedCase01 = board0.ProcessNewCase(appealCase1, new Hour(0));
            allocatedCase21 = board2.ProcessNewCase(appealCase1, new Hour(0));
        }


        [TestMethod()]
        public void InvalidConfigurationThrowsException()
        {
            try
            {
                Board invalidBoard = Board.MakeTechnicalBoard(
                    chair,
                    technicals,
                    new List<Member>(),
                    new Registrar(),
                    new ChairChooser(chair)
                    );

                Assert.Fail("Board constructor for ChairType.Technical failed to throw an exception when there are no legal members.");
            }
            catch (ArgumentException)
            { }

            try
            {
                Board invalidBoard = Board.MakeLegalBoard(
                    chair,
                    new List<Member>(),
                    legals,
                    new Registrar(),
                    new ChairChooser(chair)
                    );

                Assert.Fail("Board constructor for ChairType.Legal failed to throw an exception when there are not at least two technical members.");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod()]
        public void ProcessNewCase_LegalChair()
        {
            AllocatedCase allocatedCase = boardL.ProcessNewCase(appealCase1, new Hour(0));

            Assert.AreEqual(boardL.Chair, allocatedCase.Board.Chair.Member);
            Assert.IsTrue(boardL.Technicals.Contains(allocatedCase.Board.Rapporteur.Member));
            Assert.IsTrue(boardL.Technicals.Contains(allocatedCase.Board.OtherMember.Member));
        }

        [TestMethod()]
        public void ProcessNewCase_TechnicalChair()
        {
            AllocatedCase allocatedCase = board0.ProcessNewCase(appealCase1, new Hour(0));

            Assert.AreEqual(board0.Chair, allocatedCase.Board.Chair.Member);
            Assert.IsTrue(board0.Technicals.Contains(allocatedCase.Board.Rapporteur.Member));
            Assert.IsTrue(board0.Legals.Contains(allocatedCase.Board.OtherMember.Member));
        }



        [TestMethod()]
        public void Work_oneCase()
        {            
            foreach (Hour hour in new SimulationTimeSpan(new Hour(0), new Hour(1000)))
            {                
                board0.DoWork(hour);
                if (allocatedCase01.Stage == CaseStage.Finished)
                    break;
            }

            _case01Assertions();

        }


        [TestMethod()]
        public void Work_twoCases0()
        {
            allocatedCase02 = board0.ProcessNewCase(appealCase2, new Hour(0));

            foreach(Hour hour in new SimulationTimeSpan(new Hour(0), new Hour(1000)))
            {
                board0.DoWork(hour);
                if (allocatedCase02.Stage == CaseStage.Finished)
                    break;
            }

            _case01Assertions();
            _case02Assertions();

        }

        [TestMethod()]
        public void Work_twoCases1()
        {
            allocatedCase22 = board2.ProcessNewCase(appealCase2, new Hour(0));

            foreach (Hour hour in new SimulationTimeSpan(new Hour(0), new Hour(1000)))
            {
                board2.DoWork(hour);
                if (allocatedCase22.Stage == CaseStage.Finished)
                    break;
            }

            _case11Assertions();
            _case12Assertions();

        }







        private void _case01Assertions()
        {
            Assert.AreEqual(0, allocatedCase01.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase01.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(0, allocatedCase01.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(1, allocatedCase01.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(2, allocatedCase01.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase01.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase01.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase01.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase01.Record.ChairSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase01.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase01.Record.OP.Enqueue.Value);

            Assert.AreEqual(720, allocatedCase01.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(720, allocatedCase01.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(721, allocatedCase01.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(722, allocatedCase01.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(722, allocatedCase01.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(723, allocatedCase01.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(724, allocatedCase01.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(724, allocatedCase01.Record.ChairDecision.Start.Value);
            Assert.AreEqual(725, allocatedCase01.Record.ChairDecision.Finish.Value);


            CompletedCaseReport report = new CompletedCaseReport(allocatedCase01);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase01.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur), 
                allocatedCase01.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase01.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase01.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember), 
                allocatedCase01.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember), 
                allocatedCase01.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember), 
                allocatedCase01.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair), 
                allocatedCase01.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair), 
                allocatedCase01.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair), 
                allocatedCase01.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled, 
                allocatedCase01.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur), 
                allocatedCase01.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase01.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur), 
                allocatedCase01.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember), 
                allocatedCase01.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember) , 
                allocatedCase01.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember), 
                allocatedCase01.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair), 
                allocatedCase01.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair), 
                allocatedCase01.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair), 
                allocatedCase01.Record.ChairDecision.Finish.Value);
        }

        private void _case02Assertions()
        {
            Assert.AreEqual(0, allocatedCase02.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase02.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase02.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase02.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase02.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase02.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase02.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase02.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(6, allocatedCase02.Record.ChairSummons.Start.Value);
            Assert.AreEqual(7, allocatedCase02.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(8, allocatedCase02.Record.OP.Enqueue.Value);

            Assert.AreEqual(736, allocatedCase02.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(736, allocatedCase02.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(737, allocatedCase02.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(738, allocatedCase02.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(738, allocatedCase02.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(739, allocatedCase02.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(740, allocatedCase02.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(740, allocatedCase02.Record.ChairDecision.Start.Value);
            Assert.AreEqual(741, allocatedCase02.Record.ChairDecision.Finish.Value);

            
            CompletedCaseReport report = new CompletedCaseReport(allocatedCase02);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase02.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur), 
                allocatedCase02.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase02.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase02.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember), 
                allocatedCase02.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember), 
                allocatedCase02.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember), 
                allocatedCase02.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair), 
                allocatedCase02.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair), 
                allocatedCase02.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair), 
                allocatedCase02.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled, 
                allocatedCase02.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur), 
                allocatedCase02.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase02.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur), 
                allocatedCase02.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember), 
                allocatedCase02.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember) , 
                allocatedCase02.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember), 
                allocatedCase02.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair), 
                allocatedCase02.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair), 
                allocatedCase02.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair), 
                allocatedCase02.Record.ChairDecision.Finish.Value);
        }




        private void _case11Assertions()
        {
            Assert.AreEqual(0, allocatedCase21.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase21.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(0, allocatedCase21.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(1, allocatedCase21.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(2, allocatedCase21.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase21.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase21.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase21.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase21.Record.ChairSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase21.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase21.Record.OP.Enqueue.Value);

            Assert.AreEqual(720, allocatedCase21.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(720, allocatedCase21.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(721, allocatedCase21.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(722, allocatedCase21.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(722, allocatedCase21.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(723, allocatedCase21.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(724, allocatedCase21.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(724, allocatedCase21.Record.ChairDecision.Start.Value);
            Assert.AreEqual(725, allocatedCase21.Record.ChairDecision.Finish.Value);


            CompletedCaseReport report = new CompletedCaseReport(allocatedCase21);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase21.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair),
                allocatedCase21.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair),
                allocatedCase21.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair),
                allocatedCase21.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled,
                allocatedCase21.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur),
                allocatedCase21.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember),
                allocatedCase21.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair),
                allocatedCase21.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair),
                allocatedCase21.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair),
                allocatedCase21.Record.ChairDecision.Finish.Value);
        }

        private void _case12Assertions()
        {
            Assert.AreEqual(0, allocatedCase22.Record.Allocation.Value);
            Assert.AreEqual(0, allocatedCase22.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(2, allocatedCase22.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(3, allocatedCase22.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(4, allocatedCase22.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(4, allocatedCase22.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(5, allocatedCase22.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(6, allocatedCase22.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(6, allocatedCase22.Record.ChairSummons.Start.Value);
            Assert.AreEqual(7, allocatedCase22.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(8, allocatedCase22.Record.OP.Enqueue.Value);

            Assert.AreEqual(752, allocatedCase22.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(752, allocatedCase22.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(753, allocatedCase22.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(754, allocatedCase22.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(754, allocatedCase22.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(755, allocatedCase22.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(756, allocatedCase22.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(756, allocatedCase22.Record.ChairDecision.Start.Value);
            Assert.AreEqual(757, allocatedCase22.Record.ChairDecision.Finish.Value);


            CompletedCaseReport report = new CompletedCaseReport(allocatedCase22);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase22.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair),
                allocatedCase22.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair),
                allocatedCase22.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair),
                allocatedCase22.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled,
                allocatedCase22.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur),
                allocatedCase22.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember),
                allocatedCase22.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair),
                allocatedCase22.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair),
                allocatedCase22.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair),
                allocatedCase22.Record.ChairDecision.Finish.Value);
        }

    }
}