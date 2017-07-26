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
        Member chair ;
        Member rapporteur;
        Member other;
        MemberParameters parameters;
        MemberParameterCollection parameterCollection;
        AppealCase appealCase;
        AllocatedCase allocatedCase;
        CaseBoard caseBoard; 



        [TestInitialize]
        public void Initialise()
        {
            SimulationTime.Reset();
            WorkQueues.ClearAllQueues();

            parameters = new MemberParameters(2, 1, 2);
            parameterCollection = new MemberParameterCollection(parameters, parameters, parameters);
            chair =  new Member(parameterCollection);
            rapporteur = new Member(parameterCollection);
            other = new Member(parameterCollection);
            appealCase = new AppealCase();

            caseBoard = new CaseBoard(chair, rapporteur, other);
            allocatedCase = new AllocatedCase(appealCase, caseBoard);
        }



        [TestMethod()]
        public void EnqueueSummons()
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
            _doRapporteurWork();

            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Start);
            Assert.AreEqual(hour1, allocatedCase.Record.RapporteurSummons.Finish);
            Assert.AreEqual(0, WorkQueues.Members.Count(rapporteur));
            Assert.AreEqual(1, WorkQueues.Circulation.Count);
        }

        [TestMethod()]
        public void OtherMemberWork()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();


            Hour hour2= new Hour(2);
            Hour hour3 = new Hour(3);
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Start, "Start");
            Assert.AreEqual(hour3, allocatedCase.Record.OtherMemberSummons.Finish, "Finish");
            Assert.AreEqual(0, WorkQueues.Members.Count(other));
            Assert.AreEqual(1, WorkQueues.Circulation.Count);
        }

        [TestMethod()]
        public void ChairWork()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Start, "Start");
            Assert.AreEqual(hour5, allocatedCase.Record.ChairSummons.Finish, "Finish");
            Assert.AreEqual(0, WorkQueues.Members.Count(chair));
            Assert.AreEqual(1, WorkQueues.Circulation.Count);
        }

        [TestMethod()]
        public void EnequeueOP()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();

            Hour hour6 = new Hour(6);
            Assert.AreEqual(hour6, allocatedCase.Record.OP.Enqueue, "Enqueue");
            Assert.AreEqual(0, WorkQueues.Circulation.Count);
            //Assert.AreEqual(1, WorkQueues.OP.Count);
            Assert.AreEqual(3, WorkQueues.OPSchedule.Count);
        }



        [TestMethod()]
        public void EnequeueDecision()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();
            _incrementTimeAndSkipOP();

            Hour hour7 = new Hour(7);
            Assert.AreEqual(hour7, allocatedCase.Record.RapporteurDecision.Enqueue, "Enqueue");
        }


        [TestMethod()]
        public void RapporteurDecisionWork()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();
            _incrementTimeAndSkipOP();
            _incrementTimeAndCirculateCases();
            _doRapporteurWork();

            Hour hour8 = new Hour(8);
            Hour hour9 = new Hour(9);
            Assert.AreEqual(hour8, allocatedCase.Record.RapporteurDecision.Start, "Start");
            Assert.AreEqual(hour9, allocatedCase.Record.RapporteurDecision.Finish, "Finish");
        }

        [TestMethod()]
        public void OtherMemberDecisionWork()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();
            _incrementTimeAndSkipOP();
            _incrementTimeAndCirculateCases();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();

            Hour hour10 = new Hour(10);
            Hour hour11 = new Hour(11);
            Assert.AreEqual(hour10, allocatedCase.Record.OtherMemberDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour10, allocatedCase.Record.OtherMemberDecision.Start, "Start");
            Assert.AreEqual(hour11, allocatedCase.Record.OtherMemberDecision.Finish, "Finish");
        }

        [TestMethod()]
        public void ChairDecisionWork()
        {
            allocatedCase.EnqueueForWork();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();
            _incrementTimeAndSkipOP();
            _incrementTimeAndCirculateCases();
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();

            Hour hour12 = new Hour(12);
            Hour hour13 = new Hour(13);
            Assert.AreEqual(hour12, allocatedCase.Record.ChairDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour12, allocatedCase.Record.ChairDecision.Start, "Start");
            Assert.AreEqual(hour13, allocatedCase.Record.ChairDecision.Finish, "Finish");
        }



        private static void _incrementTimeAndSkipOP()
        {
            SimulationTime.Increment();
            //foreach (AllocatedCase ac in WorkQueues.OP.Enumeration)
            foreach(AllocatedCase ac in WorkQueues.OPSchedule.ScheduledCases)
            {
                ac.Record.SetOPStart();
                ac.Record.SetOPFinished();
                ac.EnqueueForWork();
            }
        }

        private static void _incrementTimeAndCirculateCases()
        {
            SimulationTime.Increment();
            WorkQueues.Circulation.EnqueueForNextStage();
        }


        private void _doRapporteurWork()
        {
            allocatedCase.Board.Rapporteur.Member.Work();
            SimulationTime.Increment();
            allocatedCase.Board.Rapporteur.Member.Work();
        }

        private void _doOtherMemberWork()
        {
            allocatedCase.Board.OtherMember.Member.Work();
            SimulationTime.Increment();
            allocatedCase.Board.OtherMember.Member.Work();
        }

        private void _doChairWork()
        {
            allocatedCase.Board.Chair.Member.Work();
            SimulationTime.Increment();
            allocatedCase.Board.Chair.Member.Work();
        }
    }
}