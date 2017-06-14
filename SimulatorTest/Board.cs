using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulator;
using System.Collections.Generic;

namespace SimulatorTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CanEnumerateMembers()
        {
            Chair ch = new Chair();
            TechnicalMember tm1 = new TechnicalMember();
            TechnicalMember tm2 = new TechnicalMember();
            LegalMember lm1 = new LegalMember();
            LegalMember lm2 = new LegalMember();
            LegalMember lm3 = new LegalMember();

            List<BoardMember> members = new List<BoardMember>();
            members.Add(ch);
            members.Add(tm1);
            members.Add(tm2);
            members.Add(lm1);
            members.Add(lm2);
            members.Add(lm3);
            

            Board b = new Board(
                ch,
                new List<TechnicalMember> { tm1, tm2 },
                new List<LegalMember> { lm1, lm2, lm3 }
                );


            int count = 0;
            foreach (var x in b.Members)
            {
                count++;
                Assert.IsTrue(members.Contains(x));
            }
            Assert.AreEqual(members.Count, count);
        }
    }
}
