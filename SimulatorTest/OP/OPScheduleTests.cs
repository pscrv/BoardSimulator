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
        
        private OPSchedule1 schedule0;
        private OPSchedule1 schedule1;
        

        [TestInitialize]
        public void Initialise()
        {
            schedule0 = new OPSchedule1();
            schedule1 = new OPSchedule1(1);

            chair = new Member(MemberParameterCollection.DefaultCollection());
            rapporteur = new Member(MemberParameterCollection.DefaultCollection());
            other = new Member(MemberParameterCollection.DefaultCollection());
            registar = new Registrar(new OPSchedule1());
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
                    schedule0.HasOPWork(hour, allocatedCase.Board.Chair),  
                    "Failed at " + hour);
            }
            
            for (int i = scheduleHour.Value - preparationHours; 
                i < scheduleHour.Value + TimeParameters.OPDurationInHours; 
                i++)  
            {
                hour = new Hour(i);
                Assert.IsTrue(schedule0.HasOPWork(hour, allocatedCase.Board.Chair),
                    "Failed at " + hour);
            }
            
            hour = scheduleHour.AddHours(TimeParameters.OPDurationInHours);
            Assert.IsFalse(schedule0.HasOPWork(hour, allocatedCase.Board.Chair),
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
                    schedule1.HasOPWork(hour, allocatedCase.Board.Chair),
                    "Failed at " + hour);
            }

            Hour endHour = scheduleHour.AddHours(TimeParameters.OPDurationInHours);
            for (int i = scheduleHour.Value - preparationHours;
                i < endHour.Value;
                i++)
            {
                hour = new Hour(i);
                Assert.IsTrue(schedule1.HasOPWork(hour, allocatedCase.Board.Chair),
                    "Failed at " + hour);
            }
            
            Assert.IsFalse(schedule1.HasOPWork(endHour, allocatedCase.Board.Chair),
                    "Failed at " + endHour);
        }

        [TestMethod()]
        public void Schedule0()
        {
            Hour hour = new Hour(0);

            schedule0.Schedule(hour, allocatedCase);
            schedule0.Schedule(hour, allocatedCase);
            schedule0.Schedule(hour, allocatedCase);
            List<Hour> startHours = schedule0.StartHours;

            Hour hour1 = new Hour(704);
            Hour hour2 = new Hour(720);
            Hour hour3 = new Hour(736);

            Assert.AreEqual(9, schedule0.Count);
            Assert.AreEqual(3, startHours.Count);
            Assert.IsTrue(startHours.Contains(hour1));
            Assert.IsTrue(startHours.Contains(hour2));
            Assert.IsTrue(startHours.Contains(hour3));
        }        
    }
}