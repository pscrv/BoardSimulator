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

        BoardWorker chair;
        List<BoardWorker> technicals;
        List<BoardWorker> legals;

        Board board;

        Hour h1;
        Hour h2;

        SummonsCase chSummonsCase;
        DecisionCase chDecisionCase;

        SummonsCase lgSummonsCase;
        DecisionCase lgDecisionCase;

        SummonsCase tchSummonsCase;
        DecisionCase tchDecisionCase;


        [TestInitialize]
        public void Initialize()
        {
            chParameters = new WorkParameters(6, 6, 6);
            techParameters = new WorkParameters(40, 24, 8);
            legalParameters = new WorkParameters(4, 6, 6);


            chair = new BoardWorker(chParameters, techParameters, legalParameters);
            technicals = new List<BoardWorker>
            {
                new BoardWorker(chParameters, techParameters, legalParameters),
                new BoardWorker(chParameters, techParameters, legalParameters),
            };

            legals = new List<BoardWorker>
            {
                new BoardWorker(chParameters, techParameters, legalParameters),
                new BoardWorker(chParameters, techParameters, legalParameters),
            };

            board = new Board(chair, Board.ChairType.Technical, technicals, legals);

            h1 = new Hour(0);
            h2 = new Hour(10);

            chSummonsCase = new SummonsCase();
            chDecisionCase = new DecisionCase();

            lgSummonsCase = new SummonsCase();
            lgDecisionCase = new DecisionCase();

            tchSummonsCase = new SummonsCase();
            tchDecisionCase = new DecisionCase();
        }


        [TestMethod()]
        public void BoardConstructor()
        {
            try
            {
                Board board = new Board(chair, Board.ChairType.Technical, technicals, legals);
            }
            catch (Exception e)
            {
                Assert.Fail("Board constructor threw an exception. Message : " + e.Message);
            }
        }

        [TestMethod()]
        public void DoWork_EmptyQueues()
        {
            HourlyBoardLog log = board.DoWork();

            foreach (BoardWorker worker in log.Log.Keys)
            {
                HourlyworkerLog workerlog = log.Log[worker];
                Assert.AreEqual(WorkType.NoWork, workerlog.WorkDone);
            }
        }

        [TestMethod]
        public void EnqueueCase()
        {
            Case c = new Case();
            board.EnqueueCase(c, new Hour(0));
            board.EnqueueCase(c, new Hour(1));
            board.EnqueueCase(c, new Hour(2));
            Assert.AreEqual(3, board.TotalEnqueuedCount);
        }
    }
}