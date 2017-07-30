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
        private Board board;
        
        private AppealCase appealCase;
        private AllocatedCase allocatedCase;
        
        private OPSchedule schedule;
        

        [TestInitialize]
        public void Initialise()
        {
            schedule = new OPSchedule();

            chair = new Member(MemberParameterCollection.DefaultCollection());
            rapporteur = new Member(MemberParameterCollection.DefaultCollection());
            other = new Member(MemberParameterCollection.DefaultCollection());
            board = new Board(
                chair, 
                ChairType.Technical, 
                new List<Member> { rapporteur }, 
                new List<Member> { other });

            appealCase = new AppealCase();
            allocatedCase = board.ProcessNewCase(appealCase, new Hour(0));            
        }



        [TestMethod()]
        public void HasOPWorkTest()
        {
            Hour hour;
            Hour scheduleHour = new Hour(100);
            schedule.Add(scheduleHour, allocatedCase);

            for (int i = 0; i < 96; i++)  // default chair has 4 hours of preparation
            {
                hour = new Hour(i);
                Assert.IsFalse(
                    schedule.HasOPWork(hour, allocatedCase.Board.Chair),  
                    "Failed at " + hour);
            }

            int preparationHours = allocatedCase.Board.Chair.HoursOPPreparation;
            for (int i = scheduleHour.Value - preparationHours; 
                i < scheduleHour.Value + TimeParameters.OPDurationInHours; 
                i++)  
            {
                hour = new Hour(i);
                Assert.IsTrue(schedule.HasOPWork(hour, allocatedCase.Board.Chair),
                    "Failed at " + hour);
            }

            hour = new Hour(scheduleHour.Value + TimeParameters.OPDurationInHours);
            Assert.IsFalse(schedule.HasOPWork(hour, allocatedCase.Board.Chair),
                    "Failed at " + hour);
        }

        [TestMethod()]
        public void Schedule1()
        {
            Hour hour = new Hour(0);

            schedule.Schedule(hour, allocatedCase);
            schedule.Schedule(hour, allocatedCase);
            schedule.Schedule(hour, allocatedCase);
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
    }
}