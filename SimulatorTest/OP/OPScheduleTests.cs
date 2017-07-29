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
    public class OPScheduleTests
    {
        private Member chair;
        private Member rapporteur;
        private Member other;
        private Board board;

        private CaseBoard caseboard;
        private AppealCase appealCase;
        private AllocatedCase allocatedCase;

        private Hour startHour;
        private CirculationQueue circulation;
        private OPSchedule schedule;
        private BoardQueue boardQueues;


        [TestInitialize]
        public void Initialise()
        {
            boardQueues = new BoardQueue();
            circulation = new CirculationQueue();
            schedule = new OPSchedule(circulation);

            chair = new Member(MemberParameterCollection.DefaultCollection(), boardQueues, circulation);
            rapporteur = new Member(MemberParameterCollection.DefaultCollection(), boardQueues, circulation);
            other = new Member(MemberParameterCollection.DefaultCollection(), boardQueues, circulation);
            board = new Board(
                chair, 
                ChairType.Technical, 
                new List<Member> { rapporteur }, 
                new List<Member> { other },
                boardQueues,
                null,
                circulation,
                schedule);

            caseboard = new CaseBoard(chair, rapporteur, other, boardQueues);
            appealCase = new AppealCase();
            allocatedCase = new AllocatedCase(appealCase, caseboard, SimulationTime.CurrentHour, schedule);

            startHour = new Hour(100);

            SimulationTime.Reset();
        }


        [TestMethod()]
        public void AddTest()
        {
            schedule.Add(startHour, allocatedCase);

            try
            {
                schedule.Add(startHour, allocatedCase);
                Assert.Fail("Adding a second OP for the same hour failed to throw an exception.");
            }
            catch (Exception)
            { }

            Assert.AreEqual(3, schedule.Count);
        }



        [TestMethod()]
        public void HasOPWorkTest()
        {
            schedule.Add(startHour, allocatedCase);
            Assert.IsFalse(schedule.HasOPWork(allocatedCase.Board.Chair));

            for (int i = 0; i < 95; i++)  // default chair has 4 hours of preparation
            {
                SimulationTime.Increment();
                Assert.IsFalse(
                    schedule.HasOPWork(allocatedCase.Board.Chair),  
                    "Failed at " + SimulationTime.CurrentHour);
            }

            
            for (int i = 0; i < 4 + TimeParameters.OPDurationInHours; i++)  // four hours of preparation, 
            {
                SimulationTime.Increment();
                Assert.IsTrue(schedule.HasOPWork(allocatedCase.Board.Chair),
                    "Failed at " + SimulationTime.CurrentHour);
            }

            SimulationTime.Increment();
            Assert.IsFalse(schedule.HasOPWork(allocatedCase.Board.Chair),
                    "Failed at " + SimulationTime.CurrentHour);
        }

        [TestMethod()]
        public void Schedule1()
        {
            schedule.Schedule(allocatedCase);
            schedule.Schedule(allocatedCase);
            schedule.Schedule(allocatedCase);
            List<Hour> startHours = schedule.StartHours;

            Hour hour1 = new Hour(712);
            Hour hour2 = new Hour(728);
            Hour hour3 = new Hour(744);

            Assert.AreEqual(9, schedule.Count);
            Assert.AreEqual(3, startHours.Count);
            Assert.IsTrue(startHours.Contains(hour1));
            Assert.IsTrue(startHours.Contains(hour2));
            Assert.IsTrue(startHours.Contains(hour3));
        }


        [TestMethod()]
        public void Schedule2()
        {
            Member rapporteur2 = new Member(MemberParameterCollection.DefaultCollection(), boardQueues, circulation);
            CaseBoard cb2 = new CaseBoard(chair, rapporteur2, other, boardQueues);
            AllocatedCase ac2 = new AllocatedCase(appealCase, cb2, SimulationTime.CurrentHour, schedule);


            schedule.Schedule(allocatedCase);
            schedule.Schedule(ac2);
            schedule.Schedule(allocatedCase);
            schedule.Schedule(ac2);
            List<Hour> startHours = schedule.StartHours;

            Hour hour1 = new Hour(712);
            Hour hour2 = new Hour(728);
            Hour hour3 = new Hour(744);
            Hour hour4 = new Hour(760);

            Assert.AreEqual(12, schedule.Count);
            Assert.AreEqual(4, startHours.Count);
            Assert.IsTrue(startHours.Contains(hour1));
            Assert.IsTrue(startHours.Contains(hour2));
            Assert.IsTrue(startHours.Contains(hour3));
            Assert.IsTrue(startHours.Contains(hour4));
        }
    }
}