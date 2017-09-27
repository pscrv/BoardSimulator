using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void RunOneCaseTest()
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


            Simulation simulation = new Simulation(1000, parameters, 1);
            simulation.Run();
            var x = simulation.SimulationReport;

            //TODO: add assertions
        }

        [TestMethod()]
        public void RunWithMonthlyCasesTest()
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


            Simulation simulation = new Simulation(1000, parameters, 0, 0, 1);
            simulation.Run();

            //TODO: add assertions
        }
    }
}