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
    public class ChairTests
    {
        WorkParameters parameters = new WorkParameters(10, 7, 5);
        SummonsCase sc1 = new SummonsCase();
        SummonsCase sc2 = new SummonsCase();
        DecisionCase dc1 = new DecisionCase();
        DecisionCase dc2 = new DecisionCase();
        Hour h1 = new Hour(0);
        Hour h2 = new Hour(10);
        Hour h3 = new Hour(20);
        Hour h4 = new Hour(30);

        [TestMethod()]
        public void ChairConstructor()
        {
            try
            {
                ChairWorker ch = new Chair(parameters);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in Chair constructor. Message : " + e.Message);
            }
        }



        [TestMethod()]
        public void EnqueueWork()
        {
            ChairWorker ch = new Chair(parameters);
            try
            {
                ch.EnqueueChairWork(sc1, h1);
                ch.EnqueueChairWork(sc2, h2);
                //ch.EnqueueChairWork(dc1, h3);
                //ch.EnqueueChairWork(dc2, h4);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in ChairEnqueueWork. Message : " + e.Message);
            }
        }

        [TestMethod]
        public void Dowork()
        {
            ChairWorker ch = new Chair(parameters);
            ch.EnqueueChairWork(sc1, h1);

            int count = 0;
            HourlyMemberLog log;
            bool finished = false;
            while (! finished)
            {
                log = ch.DoWork(h1);
                count++;
                if (log.WorkDone.IsFinished)
                    finished = true;
            }
            Assert.AreEqual(parameters.HoursForsummons, count);

        }
    }


    [TestClass()]
    public class TechnicalMemberTests
    {
        WorkParameters chparameters = new WorkParameters(10, 7, 5);
        WorkParameters rappparameters = new WorkParameters(12, 9, 6);

        SummonsCase chsc = new SummonsCase();
        SummonsCase rpsc = new SummonsCase();
        DecisionCase chdc = new DecisionCase();
        DecisionCase rpdc = new DecisionCase();
        Hour h1 = new Hour(0);
        Hour h2 = new Hour(10);


        [TestMethod()]
        public void TechnicalMemberTest()
        {
            try
            {
                TechnicalMember tm = new TechnicalMember(chparameters, rappparameters);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in TechnicalMember constructor. Message : " + e.Message);
            }
        }

        [TestMethod()]
        public void EnqueueWork()
        {
            TechnicalMember tm = new TechnicalMember(chparameters, rappparameters);
            try
            {
                tm.EnqueueChairWork(chsc, h1);
                tm.EnqueueChairWork(chdc, h1);
                tm.EnqueueRapporteurWork(chsc, h2);
                tm.EnqueueRapporteurWork(chdc, h2);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in ChairEnqueueWork. Message : " + e.Message);
            }
        }

        [TestMethod]
        public void Dowork()
        {
            TechnicalMember tm = new TechnicalMember(chparameters, rappparameters);
            tm.EnqueueChairWork(chsc, h1);
            tm.EnqueueChairWork(chdc, h1);
            tm.EnqueueRapporteurWork(rpsc, h2);
            tm.EnqueueRapporteurWork(rpdc, h2);

            HourlyMemberLog log;
            int count = 0;
            bool finished = false;
            while (!finished)
            {
                log = tm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(chparameters.HoursForDecision, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = tm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(chparameters.HoursForsummons, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = tm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(rappparameters.HoursForDecision, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = tm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(rappparameters.HoursForsummons, count);
        }
    }



    [TestClass()]
    public class LegalMemberTests
    {
        WorkParameters chparameters = new WorkParameters(10, 7, 5);
        WorkParameters legalparameters = new WorkParameters(12, 9, 6);

        SummonsCase chsc = new SummonsCase();
        SummonsCase lgsc = new SummonsCase();
        DecisionCase chdc = new DecisionCase();
        DecisionCase lgdc = new DecisionCase();
        Hour h1 = new Hour(0);
        Hour h2 = new Hour(10);

        [TestMethod()]
        public void LegalMemberTest()
        {
            WorkParameters chparameters = new WorkParameters(10, 7, 5);
            WorkParameters legalparameters = new WorkParameters(5, 6, 7);
            try
            {
                LegalMember lm = new LegalMember(chparameters, legalparameters);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in TechnicalMember constructor. Message : " + e.Message);
            }
        }

        [TestMethod()]
        public void EnqueueWork()
        {
            WorkParameters chparameters = new WorkParameters(10, 7, 5);
            WorkParameters legalpparameters = new WorkParameters(12, 9, 6);

            SummonsCase sc = new SummonsCase();
            DecisionCase dc = new DecisionCase();
            Hour h1 = new Hour(0);
            Hour h2 = new Hour(10);

            LegalMember lm = new LegalMember(chparameters, legalpparameters);
            try
            {
                lm.EnqueueChairWork(sc, h1);
                lm.EnqueueChairWork(dc, h1);
                lm.EnqueueLegalWork(sc, h2);
                lm.EnqueueLegalWork(dc, h2);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in ChairEnqueueWork. Message : " + e.Message);
            }
        }

        [TestMethod]
        public void Dowork()
        {
            TechnicalMember lm = new TechnicalMember(chparameters, legalparameters);
            lm.EnqueueChairWork(chsc, h1);
            lm.EnqueueChairWork(chdc, h1);
            lm.EnqueueRapporteurWork(lgsc, h2);
            lm.EnqueueRapporteurWork(lgdc, h2);

            HourlyMemberLog log;
            int count = 0;
            bool finished = false;
            while (!finished)
            {
                log = lm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(chparameters.HoursForDecision, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = lm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(chparameters.HoursForsummons, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = lm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(legalparameters.HoursForDecision, count);

            count = 0;
            finished = false;
            while (!finished)
            {
                log = lm.DoWork(h1);
                count++;
                finished = log.WorkDone.IsFinished;
            }
            Assert.AreEqual(legalparameters.HoursForsummons, count);
        }
    }
}