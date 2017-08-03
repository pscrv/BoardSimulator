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
        int initialCaseCount;


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
        }


        [TestMethod()]
        public void Constructor()
        {
            Simulation sim = new Simulation(10, parameters, 1);
        }


        [TestMethod()]
        public void Run_NoArrivingCases()
        {
            Simulation sim = new Simulation(1000, parameters, 10);
            sim.Run();
        }


        [TestMethod()]
        public void Run_WithArrivingCases()
        {
            Dictionary<int, int> arrivingCases = new Dictionary<int, int>();
            for (int i = 1; i < 5; i++)
            {
                arrivingCases.Add(i * 171, 3);
            }
            
            Simulation sim = new Simulation(1000, parameters, 10, arrivingCases);
            sim.Run();
        }


        [TestMethod()]
        public void BigRun()
        {
            Dictionary<int, int> arrivingCases = new Dictionary<int, int>();
            for (int i = 1; i < 5; i++)
            {
                arrivingCases.Add(i * 160, 1);
            }
            
            Simulation sim = new Simulation(1000, parameters, 450, arrivingCases);
            sim.Run();
        }
    }
}