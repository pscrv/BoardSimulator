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
    public class AppealCaseTests
    {
        [TestMethod()]
        public void AppealCase()
        {
            AppealCase c1 = new AppealCase();
            AppealCase c2 = new AppealCase();
            Assert.AreEqual(c1.ID + 1, c2.ID);
        }

    }
}