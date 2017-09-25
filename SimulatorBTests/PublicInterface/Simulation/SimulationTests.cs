using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorB.Tests
{
    [TestClass()]
    public class SimulationTests_B
    {
        [TestMethod()]
        public void MakeSimulationTest()
        {
            BoardParameters parameters = new TechnicalBoardParameters(
                MemberParameterCollection.DefaultCollection,
                new List<MemberParameterCollection>
                {
                    MemberParameterCollection.DefaultCollection
                },
                new List<MemberParameterCollection>
                {
                    MemberParameterCollection.DefaultCollection
                });

            try
            {
                Simulation simulation = new Simulation(0, parameters, 0);
            }
            catch (Exception e)
            {
                Assert.Fail($"Failed to construct Board. Exception: {e}");
            }
        }

        [TestMethod()]
        public void RunTest()
        {
            BoardParameters parameters = new TechnicalBoardParameters(
                MemberParameterCollection.DefaultCollection,
                new List<MemberParameterCollection>
                {
                    MemberParameterCollection.DefaultCollection
                },
                new List<MemberParameterCollection>
                {
                    MemberParameterCollection.DefaultCollection
                });


            Simulation simulation = new Simulation(10, parameters, 10);
            simulation.Run();
        }
    }
}