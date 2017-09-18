using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulatorB.Tests
{
    [TestClass()]
    public class HourTests_B
    {
        private int HoursPerDay = TimeParameters.HoursPerDay;


        private Hour zero;
        private Hour ten;

        private Hour firstHourOfDay0;
        private Hour firstHourOfWeek0;
        private Hour firstHourOfMonth0;
        private Hour firstHourOfYear0;

        private Hour firstHourOfDay1;
        private Hour firstHourOfMonth1;
        private Hour firstHourOfWeek1;
        private Hour firstHourOfYear1;

        private Hour tenthHourOfWeek1;
        private Hour tenthHourOfMonth1;
        private Hour tenthHourOfYear1;

        private Hour firstHourOfDay2;


        [TestInitialize]
        public void Initialise()
        {
            zero = new Hour(0);
            ten = new Hour(10);

            firstHourOfDay0 = new Hour(0);
            firstHourOfWeek0 = new Hour(0);
            firstHourOfMonth0 = new Hour(0);
            firstHourOfYear0 = new Hour(0);

            firstHourOfDay1 = new Hour(TimeParameters.HoursPerDay);
            firstHourOfWeek1 = new Hour(TimeParameters.HoursPerWeek);
            firstHourOfMonth1 = new Hour(TimeParameters.HoursPerMonth);
            firstHourOfYear1 = new Hour(TimeParameters.HoursPerYear);

            tenthHourOfWeek1 = new Hour(TimeParameters.HoursPerWeek + 10);
            tenthHourOfMonth1 = new Hour(TimeParameters.HoursPerMonth + 10);
            tenthHourOfYear1 = new Hour(TimeParameters.HoursPerYear + 10);

            firstHourOfDay2 = new Hour(2 * TimeParameters.HoursPerDay);
        }

        

        [TestMethod]
        public void Next()
        {
            Hour hour = zero;
            for (int i = 1; i < 10; i++)
            {
                hour = hour.Next();
                Assert.AreEqual(new Hour(i), hour);
            }
        }

        [TestMethod]
        public void Previous()
        {
            Hour hour = ten;
            for (int i = 9; i >= 0; i--)
            {
                hour = hour.Previous();
                Assert.AreEqual(new Hour(i), hour);
            }
        }

        [TestMethod()]
        public void FirstHourOfNextDayTest()
        {
            Assert.AreEqual(firstHourOfDay1, zero.FirstHourOfNextDay());
            Assert.AreEqual(firstHourOfDay2, ten.FirstHourOfNextDay());
        }

        [TestMethod()]
        public void FirstHourOfNextWeekTest()
        {
            Assert.AreEqual(firstHourOfWeek1, zero.FirstHourOfNextWeek());
            Assert.AreEqual(firstHourOfWeek1, ten.FirstHourOfNextWeek());
        }
        

        [TestMethod()]
        public void FirstHourOfNextMonthTest()
        {
            Assert.AreEqual(firstHourOfMonth1, zero.FirstHourOfNextMonth());
            Assert.AreEqual(firstHourOfMonth1, ten.FirstHourOfNextMonth());
        }

        [TestMethod()]
        public void FirstHourOfNextYearTest()
        {
            Assert.AreEqual(firstHourOfYear1, zero.FirstHourOfNextYear());
            Assert.AreEqual(firstHourOfYear1, ten.FirstHourOfNextYear());
        }

        [TestMethod()]
        public void NextFirstHourOfDayTest()
        {
            Assert.AreEqual(firstHourOfDay0, zero.NextFirstHourOfDay());
            Assert.AreEqual(firstHourOfDay2, ten.NextFirstHourOfDay());
        }

        [TestMethod()]
        public void NextFirstHourOfWeekTest()
        {
            Assert.AreEqual(firstHourOfWeek0, zero.NextFirstHourOfWeek());
            Assert.AreEqual(firstHourOfWeek1, ten.NextFirstHourOfWeek());
        }

        [TestMethod()]
        public void NextFirstHourOfMonthTest()
        {
            Assert.AreEqual(firstHourOfMonth0, zero.NextFirstHourOfMonth());
            Assert.AreEqual(firstHourOfMonth1, ten.NextFirstHourOfMonth());
        }

        [TestMethod()]
        public void NextFirstHourOfYearTest()
        {
            Assert.AreEqual(firstHourOfYear0, zero.NextFirstHourOfYear());
            Assert.AreEqual(firstHourOfYear1, ten.NextFirstHourOfYear());
        }

        [TestMethod()]
        public void AddHoursTest()
        {
            Assert.AreEqual(ten, zero.AddHours(10));
        }

        [TestMethod()]
        public void AddDaysTest()
        {
            Assert.AreEqual(firstHourOfDay1, zero.AddDays(1));
            Assert.AreEqual(firstHourOfDay2, zero.AddDays(2));
        }

        [TestMethod()]
        public void AddMonthsTest()
        {
            Assert.AreEqual(firstHourOfMonth1, zero.AddMonths(1));
            Assert.AreEqual(tenthHourOfMonth1, ten.AddMonths(1));
        }

        [TestMethod()]
        public void SubtractHoursTest()
        {
            Assert.AreEqual(zero, ten.SubtractHours(10));
        }

        [TestMethod()]
        public void SubtractDaysTest()
        {
            Assert.AreEqual(zero, firstHourOfDay1.SubtractDays(1));
        }

        [TestMethod()]
        public void SubtractMonthsTest()
        {
            Assert.AreEqual(zero, firstHourOfMonth1.SubtractMonths(1));
            Assert.AreEqual(ten, tenthHourOfMonth1.SubtractMonths(1));
        }
        
    }
}