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
    public class BWorkerTests
    {
        WorkParameters chairparameters;
        WorkParameters rapporteurparameters;
        WorkParameters otherparameters;
        BoardWorker bw;

        [TestInitialize]
        public void Initialize()
        {
            chairparameters = new WorkParameters(4, 5, 6);
            rapporteurparameters = new WorkParameters(8, 9, 10);
            otherparameters = new WorkParameters(3, 4, 5);

            bw = new BoardWorker(chairparameters, rapporteurparameters, otherparameters);
        }
        

        [TestMethod()]
        public void BWorkerTest()
        {

            try
            {
                BoardWorker bw = new BoardWorker(chairparameters, rapporteurparameters, otherparameters);
            }
            catch (Exception e)
            {
                Assert.Fail("BWorker threw an exception. Message : " + e.Message);
            }
        }

        [TestMethod()]
        public void EnqueueChairWorkTest()
        {
            DecisionCase dc = new DecisionCase();
            SummonsCase sc = new SummonsCase();
            Hour h = new Hour(0);
            bw.EnqueueChairWork(dc, h);
            bw.EnqueueChairWork(sc, h);
            Assert.AreEqual(2, bw.TotalWorkCount);
        }

        [TestMethod()]
        public void EnqueueRapporteurWorkTest()
        {
            DecisionCase dc = new DecisionCase();
            SummonsCase sc = new SummonsCase();
            Hour h = new Hour(0);
            bw.EnqueueRapporteurWork(dc, h);
            bw.EnqueueRapporteurWork(sc, h);
            Assert.AreEqual(2, bw.TotalWorkCount);
        }

        [TestMethod()]
        public void EnqueueOtherWorkTest()
        {
            DecisionCase dc = new DecisionCase();
            SummonsCase sc = new SummonsCase();
            Hour h = new Hour(0);
            bw.EnqueueOtherWork(dc, h);
            bw.EnqueueOtherWork(sc, h);
            Assert.AreEqual(2, bw.TotalWorkCount);
        }

        [TestMethod()]
        public void DoWorkTest()
        {
            DecisionCase dc1 = new DecisionCase();
            DecisionCase dc2 = new DecisionCase();
            DecisionCase dc3 = new DecisionCase();
            SummonsCase sc1 = new SummonsCase();
            SummonsCase sc2 = new SummonsCase();
            SummonsCase sc3 = new SummonsCase();
            Hour h = new Hour(0);
            bw.EnqueueChairWork(dc1, h);
            bw.EnqueueChairWork(sc1, h);
            bw.EnqueueRapporteurWork(dc2, h);
            bw.EnqueueRapporteurWork(sc2, h);
            bw.EnqueueOtherWork(dc3, h);
            bw.EnqueueOtherWork(sc3, h);

            Assert.AreEqual(6, bw.TotalWorkCount);

            HourlyworkerLog log;
            bool isMoreWork = true;
            int count = 0;
            while (isMoreWork)
            {
                count++;
                log = bw.DoWork();
                isMoreWork = log.WorkDone != WorkType.NoWork;
            }
            Assert.AreEqual(34, count);

        }
    }
}