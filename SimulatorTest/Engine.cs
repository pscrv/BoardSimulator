using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulator;

namespace SimulatorTests
{
    [TestClass]
    public class EngineTests
    {
        [TestMethod]
        public void TestEngine()
        {
            Engine e = new Engine(10);
            e.Run();
        }
    }
}
