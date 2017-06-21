using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{
    [TestClass()]
    public class TimespanTests
    {
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            int length = 5;
            Timespan span = new Timespan(length);
            int count = 0;
            foreach (Hour hour in span)
            {
                Assert.AreEqual(hour.Value, count);
                count++;
            }
            Assert.AreEqual(length, count);
        }
    }
}