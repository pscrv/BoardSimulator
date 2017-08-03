using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{

    [TestClass()]
    public class BoardTests
    {
        Board board;

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
            parameterCollection = new MemberParameterCollection(memberParameters, memberParameters, memberParameters);

            chair = new Member(parameterCollection);
            technicals = new List<Member> { new Member(parameterCollection) };
            legals = new List<Member> { new Member(parameterCollection) };

            board = new Board(
                chair, 
                ChairType.Technical, 
                technicals, 
                legals);
            allocatedCase1 = board.ProcessNewCase(appealCase1, new Hour(0));
        }



        [TestMethod()]
        public void Work_oneCase()
        {            
            foreach (Hour hour in new SimulationTimeSpan(new Hour(0), new Hour(1000)))
            {                
                board.DoWork(hour);
                if (allocatedCase1.Stage == CaseStage.Finished)
                    break;
            }

            _case1Assertions();

        }


        [TestMethod()]
        public void Work_twoCases()
        {
            allocatedCase2 = board.ProcessNewCase(appealCase2, new Hour(0));

            foreach(Hour hour in new SimulationTimeSpan(new Hour(0), new Hour(1000)))
            {
                board.DoWork(hour);
                if (allocatedCase2.Stage == CaseStage.Finished)
                    break;
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

            Assert.AreEqual(720, allocatedCase1.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(720, allocatedCase1.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(721, allocatedCase1.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(722, allocatedCase1.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(722, allocatedCase1.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(723, allocatedCase1.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(724, allocatedCase1.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(724, allocatedCase1.Record.ChairDecision.Start.Value);
            Assert.AreEqual(725, allocatedCase1.Record.ChairDecision.Finish.Value);


            CompletedCaseReport report = new CompletedCaseReport(allocatedCase1);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase1.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur), 
                allocatedCase1.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase1.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase1.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember), 
                allocatedCase1.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember), 
                allocatedCase1.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember), 
                allocatedCase1.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair), 
                allocatedCase1.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair), 
                allocatedCase1.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair), 
                allocatedCase1.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled, 
                allocatedCase1.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur), 
                allocatedCase1.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase1.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur), 
                allocatedCase1.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember), 
                allocatedCase1.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember) , 
                allocatedCase1.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember), 
                allocatedCase1.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair), 
                allocatedCase1.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair), 
                allocatedCase1.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair), 
                allocatedCase1.Record.ChairDecision.Finish.Value);
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

            Assert.AreEqual(736, allocatedCase2.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(736, allocatedCase2.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(737, allocatedCase2.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(738, allocatedCase2.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(738, allocatedCase2.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(739, allocatedCase2.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(740, allocatedCase2.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(740, allocatedCase2.Record.ChairDecision.Start.Value);
            Assert.AreEqual(741, allocatedCase2.Record.ChairDecision.Finish.Value);

            
            CompletedCaseReport report = new CompletedCaseReport(allocatedCase2);
            Assert.AreEqual(report.HourOfAlloction, allocatedCase2.Record.Allocation.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Rapporteur), 
                allocatedCase2.Record.RapporteurSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase2.Record.RapporteurSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Rapporteur),
                allocatedCase2.Record.RapporteurSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.OtherMember), 
                allocatedCase2.Record.OtherMemberSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.OtherMember), 
                allocatedCase2.Record.OtherMemberSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.OtherMember), 
                allocatedCase2.Record.OtherMemberSummons.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForSummons(WorkerRole.Chair), 
                allocatedCase2.Record.ChairSummons.Enqueue.Value);
            Assert.AreEqual(
                report.HourSummonsWorkStarted(WorkerRole.Chair), 
                allocatedCase2.Record.ChairSummons.Start.Value);
            Assert.AreEqual(
                report.HourSummonsWorkFinished(WorkerRole.Chair), 
                allocatedCase2.Record.ChairSummons.Finish.Value);
            Assert.AreEqual(
                report.HourOPScheduled, 
                allocatedCase2.Record.OP.Enqueue.Value);

            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Rapporteur), 
                allocatedCase2.Record.RapporteurDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Rapporteur), 
                allocatedCase2.Record.RapporteurDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Rapporteur), 
                allocatedCase2.Record.RapporteurDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.OtherMember), 
                allocatedCase2.Record.OtherMemberDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.OtherMember) , 
                allocatedCase2.Record.OtherMemberDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.OtherMember), 
                allocatedCase2.Record.OtherMemberDecision.Finish.Value);
            Assert.AreEqual(
                report.HourEnqueuedForDecision(WorkerRole.Chair), 
                allocatedCase2.Record.ChairDecision.Enqueue.Value);
            Assert.AreEqual(
                report.HourDecisionWorkStarted(WorkerRole.Chair), 
                allocatedCase2.Record.ChairDecision.Start.Value);
            Assert.AreEqual(
                report.HourDecisionWorkFinished(WorkerRole.Chair), 
                allocatedCase2.Record.ChairDecision.Finish.Value);
        }

    }
}