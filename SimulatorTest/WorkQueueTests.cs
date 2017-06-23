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
    public class WorkQueueTests
    {
        [TestMethod()]
        public void SummonsQueueTest()
        {
            try
            {
                SummonsQueue sq = new SummonsQueue(); 
            }
            catch (Exception e)
            {
                Assert.Fail("new SummonsQueue() threw an exception with message : " + e.Message);
            }
        }

        [TestMethod()]
        public void DecisionQueueTest()
        {
            try
            {
                DecisionQueue dq = new DecisionQueue();
            }
            catch (Exception e)
            {
                Assert.Fail("new DecisionQueue() threw an exception with message : " + e.Message);
            }
        }

        [TestMethod()]
        public void EnqueueTest()
        {
            SummonsQueue sq = new SummonsQueue();
            DecisionQueue dq = new DecisionQueue();

            Timespan span = new Timespan(5);
            foreach (Hour h in span)
            {
                try
                {
                    sq.Enqueue(new SummonsWork(new SummonsCase(), 1), h);
                }
                catch (Exception e)
                {
                    Assert.Fail("SummonsQueue.Enqueue threw an exception with message : " + e.Message);
                }

                try
                {
                    dq.Enqueue(new DecisionWork(new DecisionCase(), 1), h);
                }
                catch (Exception e)
                {
                    Assert.Fail("DecisionQueue.Enqueue threw an exception with message : " + e.Message);
                }
            }            
        }

        [TestMethod()]
        public void DequeueTest()
        {
            SummonsQueue sq = new SummonsQueue();
            DecisionQueue dq = new DecisionQueue();

            Timespan span = new Timespan(5);
            List<Work> summonsWork = new List<Work>();
            List<Work> decisionWork = new List<Work>();

            SummonsWork sw;
            DecisionWork dw;
            foreach (Hour h in span)
            {
                sw = new SummonsWork(new SummonsCase(), 1);
                dw = new DecisionWork(new DecisionCase(), 1);
                summonsWork.Add(sw);
                decisionWork.Add(dw);

                sq.Enqueue(sw, h);
                dq.Enqueue(dw, h);
            }

            foreach (Work work in summonsWork)
            {
                Assert.AreEqual(work, sq.Dequeue());
            }

            foreach (Work work in decisionWork)
            {
                Assert.AreEqual(work, dq.Dequeue());
            }
        }

        [TestMethod()]
        public void AgeOfOldestTest()
        {
            List<Hour> hours = new List<Hour> { new Hour(0), new Hour(10), new Hour(15) };
            SummonsQueue sq = new SummonsQueue();
            foreach (Hour h in hours)
            {
                sq.Enqueue(new SummonsWork(new SummonsCase(), 1), h);
                Assert.AreEqual(hours[0], sq.AgeOfOldest());
            }

            sq.Dequeue();
            Assert.AreEqual(hours[1], sq.AgeOfOldest());
            


        }
    }
}