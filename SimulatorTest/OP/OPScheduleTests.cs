using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{
    [TestClass()]
    public class OPScheduleTests
    {
        private Member chair;
        private Member rapporteur;
        private Member other;
        private Registrar registar;
        private Board board;

        private AppealCase appealCase;
        private AllocatedCase allocatedCase;

        private OPSchedule schedule0;
        private OPSchedule schedule1;


        [TestInitialize]
        public void Initialise()
        {
            schedule0 = new SimpleOPScheduler();
            schedule1 = new SimpleOPScheduler(1);

            chair = new Member(MemberParameterCollection.DefaultCollection());
            rapporteur = new Member(MemberParameterCollection.DefaultCollection());
            other = new Member(MemberParameterCollection.DefaultCollection());
            registar = new Registrar(new SimpleOPScheduler());
            board = new Board(
                chair,
                ChairType.Technical,
                new List<Member> { rapporteur },
                new List<Member> { other },
                registar,
                new ChairChooser(chair));

            appealCase = new AppealCase();
            allocatedCase = board.ProcessNewCase(appealCase, new Hour(0));
        }



        [TestMethod()]
        public void HasOPWorkTest0()
        {
            Hour hour;
            Hour scheduleHour = new Hour(100);
            schedule0.Add(scheduleHour, allocatedCase);

            int preparationHours = chair.GetParameters(WorkerRole.Chair).HoursOPPrepration;
            for (int i = 0; i < scheduleHour.Value - preparationHours; i++)  // default chair has 4 hours of preparation
            {
                hour = new Hour(i);
                Assert.IsFalse(
                    schedule0.HasOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
            }

            for (int i = scheduleHour.Value - preparationHours;
                i < scheduleHour.Value + TimeParameters.OPDurationInHours;
                i++)
            {
                hour = new Hour(i);
                Assert.IsTrue(schedule0.HasOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
            }

            hour = scheduleHour.AddHours(TimeParameters.OPDurationInHours);
            Assert.IsFalse(schedule0.HasOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
        }

        [TestMethod()]
        public void HasOPWorkTest1()
        {
            Hour hour;
            Hour scheduleHour = new Hour(100);
            schedule1.Add(scheduleHour, allocatedCase);

            int preparationHours = chair.GetParameters(WorkerRole.Chair).HoursOPPrepration;
            for (int i = 0; i < scheduleHour.Value - preparationHours; i++)  // default chair has 4 hours of preparation
            {
                hour = new Hour(i);
                Assert.IsFalse(
                    schedule1.HasOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
            }

            Hour endHour = scheduleHour.AddHours(TimeParameters.OPDurationInHours);
            for (int i = scheduleHour.Value - preparationHours;
                i < endHour.Value;
                i++)
            {
                hour = new Hour(i);
                Assert.IsTrue(schedule1.HasOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
            }

            Assert.IsFalse(schedule1.HasOPWork(endHour, allocatedCase.Board.Chair.Member),
                    "Failed at " + endHour);
        }

        [TestMethod()]
        public void GetOPWorkTest0()
        {
            Hour scheduleHour = new Hour(100);
            Hour endHour = scheduleHour.AddHours(TimeParameters.OPDurationInHours);
            int preparationHours = chair.GetParameters(WorkerRole.Chair).HoursOPPrepration;

            schedule0.Add(scheduleHour, allocatedCase);
            
            foreach (Hour hour in 
                new SimulationTimeSpan(
                    new Hour(0),
                    scheduleHour.SubtractHours(preparationHours).Previous()))
            {
                Assert.IsNull(
                    schedule0.GetOPWork(hour, allocatedCase.Board.Chair.Member),
                    "Failed at " + hour);
            }

            foreach (Hour hour in
                new SimulationTimeSpan(
                    scheduleHour.SubtractHours(preparationHours),
                    endHour.Previous()))
            {
                Assert.AreEqual(
                    schedule0.GetOPWork(hour, allocatedCase.Board.Chair.Member),
                    allocatedCase,
                    "Failed at " + hour);
            }

            Assert.IsNull(schedule0.GetOPWork(endHour, allocatedCase.Board.Chair.Member),
                    "Failed at " + endHour);



        }

        
        [TestMethod()]
        public void Schedule0()
        {
            Hour hour = new Hour(0);

            schedule0.Schedule(hour, allocatedCase);
            schedule0.Schedule(hour, allocatedCase);
            schedule0.Schedule(hour, allocatedCase);

            Hour hour1 = new Hour(704);
            Hour hour2 = new Hour(720);
            Hour hour3 = new Hour(736);
            List<Hour> startHours = schedule0.StartHours;

            Assert.AreEqual(3, schedule0.Count);
            Assert.AreEqual(3, startHours.Count);
            Assert.IsTrue(startHours.Contains(hour1));
            Assert.IsTrue(startHours.Contains(hour2));
            Assert.IsTrue(startHours.Contains(hour3));
        }

        [TestMethod()]
        public void UpdateScheduleAndGetFinishedCases0()
        {
            Hour hour = new Hour(0);
            allocatedCase.Record.SetOPEnqueue(hour);

            schedule0.Schedule(hour, allocatedCase);
            
            schedule0.UpdateScheduleAndGetFinishedCases(new Hour(704));
            List<AllocatedCase> runningCases = schedule0.RunningCases;
            Assert.AreEqual(1, runningCases.Count);  //TODO: change this when RunningCases is in the base class
            Assert.IsTrue(runningCases.Contains(allocatedCase));
            Assert.AreEqual(0, schedule0.Count);
            Assert.AreEqual(0, schedule0.StartHours.Count);

            schedule0.UpdateScheduleAndGetFinishedCases(new Hour(712));
            runningCases = schedule0.RunningCases;
            Assert.AreEqual(0, runningCases.Count);  //TODO: change this when RunningCases is in the base class
            
        }
    }
}
