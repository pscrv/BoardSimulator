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
    public class TimelineTests
    {
        [TestMethod()]
        public void TimelineTest()
        {
            int length = 5;
            Timeline tl = new Timeline(new Timespan(length));

            int count = 0;
            foreach (Hour hour in tl.Span)
            {
                Assert.AreEqual(count, hour.Value);
                count++;
            }
            Assert.AreEqual(length, count);
        }
    }
}