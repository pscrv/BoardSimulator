﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        Member chair;
        List<Member> technical;
        List<Member> legal;

        Board board;
        List<AppealCase> initialCases;
        int hours;


        [TestInitialize]
        public void Initialize()
        {
            chair = new Member();
            technical = new List<Member> { new Member() };
            legal = new List<Member> { new Member() };

            board = new Board(chair, technical, legal);
            initialCases = new List<AppealCase>();
            hours = 0;
        }


        [TestMethod()]
        public void Constructor()
        {
            try
            {
                board = new Board(chair, technical, legal);
            }
            catch (Exception e)
            {
                Assert.Fail("Constructor threw an exception: " + e.Message);
            }
        }

        [TestMethod]
        public void EnqueueNew()
        {
            int howmany = 4;

            for (int i = 0; i < howmany; i++)
            {
                board.EnqueueNewCase(new AppealCase());
            }

            Assert.AreEqual(howmany, board.IncomingCaseCount);

        }

    }
}