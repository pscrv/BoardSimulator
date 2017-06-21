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
        SummonsWork sw1 = new SummonsWork();
        SummonsWork sw2 = new SummonsWork();
        DecisionWork dw1 = new DecisionWork();
        DecisionWork dw2 = new DecisionWork();
        Hour h1 = new Hour(0);
        Hour h2 = new Hour(10);
        Hour h3 = new Hour(20);
        Hour h4 = new Hour(30);

        [TestMethod()]
        public void ChairConstructor()
        {
            try
            {
                BoardMember ch = new Chair(parameters);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in Chair constructor. Message : " + e.Message);
            }
        }



        [TestMethod()]
        public void EnqueueWork()
        {
            BoardMember ch = new Chair(parameters);
            try
            {
                ch.EnqueueChairWork(sw1, h1);
                ch.EnqueueChairWork(sw2, h2);
                ch.EnqueueChairWork(dw1, h3);
                ch.EnqueueChairWork(dw2, h4);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in ChairEnqueueWork. Message : " + e.Message);
            }
        }

        [TestMethod]
        public void Dowork()
        {
            BoardMember ch = new Chair(parameters);
            ch.EnqueueChairWork(sw1, h1);

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

        SummonsWork chsw = new SummonsWork();
        SummonsWork rpsw = new SummonsWork();
        DecisionWork chdw = new DecisionWork();
        DecisionWork rpdw = new DecisionWork();
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
                tm.EnqueueChairWork(chsw, h1);
                tm.EnqueueChairWork(chdw, h1);
                tm.EnqueueRapporteurWork(chsw, h2);
                tm.EnqueueRapporteurWork(chdw, h2);
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
            tm.EnqueueChairWork(chsw, h1);
            tm.EnqueueChairWork(chdw, h1);
            tm.EnqueueRapporteurWork(rpsw, h2);
            tm.EnqueueRapporteurWork(rpdw, h2);

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
        [TestMethod()]
        public void LegalMemberTest()
        {
            WorkParameters chparameters = new WorkParameters(10, 7, 5);
            WorkParameters rappparameters = new WorkParameters(5, 6, 7);
            try
            {
                LegalMember lm = new LegalMember(chparameters, rappparameters);
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

            SummonsWork sw = new SummonsWork();
            DecisionWork dw = new DecisionWork();
            Hour h1 = new Hour(0);
            Hour h2 = new Hour(10);

            LegalMember tm = new LegalMember(chparameters, legalpparameters);
            try
            {
                tm.EnqueueChairWork(sw, h1);
                tm.EnqueueChairWork(dw, h1);
                tm.EnqueueLegalWork(sw, h2);
                tm.EnqueueLegalWork(dw, h2);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception in ChairEnqueueWork. Message : " + e.Message);
            }
        }
    }
}