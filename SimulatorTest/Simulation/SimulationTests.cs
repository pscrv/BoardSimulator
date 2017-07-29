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
    public class SimulationTests
    {
        BoardParameters parameters;
        List<AppealCase> caseList;

        [TestInitialize]
        public void Initialise()
        {
            ChairType type = ChairType.Technical;
            MemberParameterCollection chair = new MemberParameterCollection(
                new MemberParameters(6, 6, 12),
                new MemberParameters(40, 8, 24),
                new MemberParameters(3, 4, 8));

            List<MemberParameterCollection> technicals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10))
            };

            List<MemberParameterCollection> legals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10)),

                new MemberParameterCollection(
                    new MemberParameters(9, 9, 15),
                    new MemberParameters(43, 11, 27),
                    new MemberParameters(6, 7, 11))
            };

            parameters = new BoardParameters(
                type,
                chair,
                technicals,
                legals);

            caseList = new List<AppealCase> { new AppealCase() };
            
        }

        [TestMethod()]
        public void Constructor()
        {
            Simulation sim = new Simulation(10, parameters, caseList);
        }

        [TestMethod()]
        public void Run()
        {
            Simulation sim = new Simulation(1000, parameters, caseList);
            sim.Run();
        }
    }
}