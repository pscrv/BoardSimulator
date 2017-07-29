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

        BoardQueue boardQueues;
        CirculationQueue circulation;
        OPSchedule opSchedule;



        [TestInitialize]
        public void Initialise()
        {
            SimulationTime.Reset();

            boardQueues = new BoardQueue();
            circulation = new CirculationQueue();
            opSchedule = new OPSchedule(circulation);

            parameters = new MemberParameters(2, 1, 2);
            parameterCollection = new MemberParameterCollection(parameters, parameters, parameters);
            chair =  new Member(parameterCollection, boardQueues, circulation);
            rapporteur = new Member(parameterCollection, boardQueues, circulation);
            other = new Member(parameterCollection, boardQueues, circulation);
            appealCase = new AppealCase();

            caseBoard = new CaseBoard(chair, rapporteur, other, boardQueues);
            allocatedCase = new AllocatedCase(appealCase, caseBoard, SimulationTime.CurrentHour, opSchedule);
        }



        [TestMethod()]
        public void EnqueueSummons()
        {
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);

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
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
            _doRapporteurWork();

            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Start);
            Assert.AreEqual(hour1, allocatedCase.Record.RapporteurSummons.Finish);
            Assert.AreEqual(0, boardQueues.Count(rapporteur));
            Assert.AreEqual(1, circulation.Count);
        }

        [TestMethod()]
        public void OtherMemberWork()
        {
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();


            Hour hour2= new Hour(2);
            Hour hour3 = new Hour(3);
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Start, "Start");
            Assert.AreEqual(hour3, allocatedCase.Record.OtherMemberSummons.Finish, "Finish");
            Assert.AreEqual(0, boardQueues.Count(other));
            Assert.AreEqual(1, circulation.Count);
        }

        [TestMethod()]
        public void ChairWork()
        {
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
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
            Assert.AreEqual(0, boardQueues.Count(chair));
            Assert.AreEqual(1, circulation.Count);
        }

        [TestMethod()]
        public void EnequeueOP()
        {
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
            _doRapporteurWork();
            _incrementTimeAndCirculateCases();
            _doOtherMemberWork();
            _incrementTimeAndCirculateCases();
            _doChairWork();
            _incrementTimeAndCirculateCases();

            Hour hour6 = new Hour(6);
            Assert.AreEqual(hour6, allocatedCase.Record.OP.Enqueue, "Enqueue");
            Assert.AreEqual(0, circulation.Count);
            Assert.AreEqual(3, opSchedule.Count);
        }



        [TestMethod()]
        public void EnequeueDecision()
        {
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
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
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
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
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
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
            allocatedCase.EnqueueForWork(SimulationTime.CurrentHour);
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

        
        private void _incrementTimeAndSkipOP()
        {
            SimulationTime.Increment();
            foreach(AllocatedCase ac in opSchedule.ScheduledCases)
            {
                ac.Record.SetOPStart(SimulationTime.CurrentHour);
                ac.Record.SetOPFinished(SimulationTime.CurrentHour);
                ac.EnqueueForWork(SimulationTime.CurrentHour);
            }
        }

        private void _incrementTimeAndCirculateCases()
        {
            SimulationTime.Increment();
            circulation.EnqueueForNextStage(SimulationTime.CurrentHour);
        }


        private void _doRapporteurWork()
        {
            allocatedCase.Board.Rapporteur.Member.Work(SimulationTime.CurrentHour);
            SimulationTime.Increment();
            allocatedCase.Board.Rapporteur.Member.Work(SimulationTime.CurrentHour);
        }

        private void _doOtherMemberWork()
        {
            allocatedCase.Board.OtherMember.Member.Work(SimulationTime.CurrentHour);
            SimulationTime.Increment();
            allocatedCase.Board.OtherMember.Member.Work(SimulationTime.CurrentHour);
        }

        private void _doChairWork()
        {
            allocatedCase.Board.Chair.Member.Work(SimulationTime.CurrentHour);
            SimulationTime.Increment();
            allocatedCase.Board.Chair.Member.Work(SimulationTime.CurrentHour);
        }
    }
}