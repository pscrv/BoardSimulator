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
    public class BoardTests
    {
        WorkParameters chParameters;
        WorkParameters techParameters;
        WorkParameters legalParameters;

        Chair chair;
        List<TechnicalMember> technicals;
        List<LegalMember> legals;

        Board board;

        [TestInitialize]
        public void Initialize()
        {
            chParameters = new WorkParameters(6, 6, 6);
            techParameters = new WorkParameters(40, 24, 8);
            legalParameters = new WorkParameters(4, 6, 6);


            chair = new Chair(chParameters);
            technicals = new List<TechnicalMember>
            {
                new TechnicalMember(chParameters, techParameters),
                new TechnicalMember(chParameters, techParameters),
                new TechnicalMember(chParameters, techParameters),
                new TechnicalMember(chParameters, techParameters),
            };

            legals = new List<LegalMember>
            {
                new LegalMember(chParameters, legalParameters),
                new LegalMember(chParameters, legalParameters),
                new LegalMember(chParameters, legalParameters),
            };
            
            board = new Board(chair, technicals, legals);
        }


        [TestMethod()]
        public void BoardConstructor()
        {
            try
            {
                Board board = new Board(chair, technicals, legals);
            }
            catch (Exception e)
            {
                Assert.Fail("Board constructor threw an exception. Message : " + e.Message);
            }
        }

        [TestMethod()]
        public void DoWork()
        {
            Hour hour = new Hour(1);

            board.DoWork(hour);
            board.DM(hour);
        }
    }
}