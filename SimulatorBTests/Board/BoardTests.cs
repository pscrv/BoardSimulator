using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulatorB.Tests
{
    [TestClass()]
    public class BoardTests_B
    {
        Board boardT = new TechnicalBoard(
                    new Member(MemberParameterCollection.FastCollection),
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.FastCollection)
                    },
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.FastCollection)
                    }
                    );

        Board boardL = new LegalBoard(
                    new Member(MemberParameterCollection.FastCollection),
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.FastCollection),
                        new Member(MemberParameterCollection.FastCollection)
                    },
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.FastCollection)
                    }
                    );


        [TestMethod()]
        public void InvalidConfigurationThrowsException()
        {
            try
            {
                Board invalidBoard = new TechnicalBoard(
                    new Member(MemberParameterCollection.DefaultCollection),
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.DefaultCollection)
                    },
                    new List<Member>()
                    );

                Assert.Fail("Board constructor for ChairType.Technical failed to throw an exception when there are no legal members.");
            }
            catch (ArgumentException)
            { }

            try
            {
                Board invalidBoard = new LegalBoard(
                    new Member(MemberParameterCollection.DefaultCollection),
                    new List<Member>(),
                    new List<Member>
                    {
                        new Member(MemberParameterCollection.DefaultCollection)
                    }
                    );

                Assert.Fail("Board constructor for ChairType.Legal failed to throw an exception when there are not at least two technical members.");
            }
            catch (ArgumentException)
            { }
        }


        [TestMethod()]
        public void ProcessNewCaseTest()
        {
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, new Hour(0));

            Assert.AreEqual(1, boardT.CirculatingSummonsCount);
        }

        [TestMethod()]
        public void DoWork_SummonsCirculationTest()
        {
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, new Hour(0));
            Assert.AreEqual(1, boardT.CirculatingSummonsCount); 

            boardT.DoWork(new Hour(0));
            Assert.AreEqual(1, boardT.CirculatingSummonsCount);
            boardT.DoWork(new Hour(1));
            Assert.AreEqual(1, boardT.CirculatingSummonsCount);
            boardT.DoWork(new Hour(2));
            Assert.AreEqual(0, boardT.CirculatingSummonsCount);
        }


        [TestMethod()]
        public void DoWork_ScheduleOPTestB()
        {
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, new Hour(0));
            boardT.DoWork(new Hour(0));
            Assert.AreEqual(0, boardT.PendingOPCount);
            boardT.DoWork(new Hour(1));
            Assert.AreEqual(0, boardT.PendingOPCount);
            boardT.DoWork(new Hour(2));
            Assert.AreEqual(1, boardT.PendingOPCount);
            boardT.DoWork(new Hour(3));
            Assert.AreEqual(1, boardT.PendingOPCount);
        }

        [TestMethod()]
        public void OPWorkTest()
        {
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, new Hour(0));
            boardT.DoWork(new Hour(0));
            boardT.DoWork(new Hour(1));
            boardT.DoWork(new Hour(2));

            boardT.DoWork(new Hour(710));
            Assert.AreEqual(0, boardT.RunningOPCount);
            boardT.DoWork(new Hour(711));
            Assert.AreEqual(0, boardT.RunningOPCount);
            boardT.DoWork(new Hour(712));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(713));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(714));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(715));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(716));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(717));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(718));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(719));
            Assert.AreEqual(1, boardT.RunningOPCount);
            boardT.DoWork(new Hour(720));
            Assert.AreEqual(0, boardT.RunningOPCount);
        }

        [TestMethod()]
        public void DecisionCirculationTest()
        {
            Hour hour = new Hour(0);
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, hour);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.CirculatingDecisionsCount);

            hour = new Hour(1);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.CirculatingDecisionsCount);

            hour = new Hour(2);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.CirculatingDecisionsCount);

            
            for (int i = 711; i < 719; i++)
            {
                hour = new Hour(i);
                boardT.DoWork(hour);
                Assert.AreEqual(0, boardT.CirculatingDecisionsCount);
            }

            hour = new Hour(719);
            boardT.DoWork(hour);
            Assert.AreEqual(1, boardT.CirculatingDecisionsCount);

            hour = new Hour(720);
            boardT.DoWork(hour);
            Assert.AreEqual(1, boardT.CirculatingDecisionsCount);

            hour = new Hour(721);
            boardT.DoWork(new Hour(721));
            Assert.AreEqual(1, boardT.CirculatingDecisionsCount);

            hour = new Hour(722);
            boardT.DoWork(new Hour(722));
            Assert.AreEqual(0, boardT.CirculatingDecisionsCount);

            hour = new Hour(723);
            boardT.DoWork(new Hour(723));
            Assert.AreEqual(0, boardT.CirculatingDecisionsCount);
        }


        [TestMethod()]
        public void FinishedCaseTest()
        {
            Hour hour = new Hour(0);
            AppealCase appealCase = new AppealCase();
            boardT.ProcessNewCase(appealCase, hour);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.FinishedCaseCount);

            hour = new Hour(1);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.FinishedCaseCount);

            hour = new Hour(2);
            boardT.DoWork(hour);
            Assert.AreEqual(0, boardT.FinishedCaseCount);
            

            for (int i = 711; i < 722; i++)
            {
                hour = new Hour(i);
                boardT.DoWork(hour);
                Assert.AreEqual(0, boardT.FinishedCaseCount);
            }
            

            boardT.DoWork(new Hour(722));
            Assert.AreEqual(1, boardT.FinishedCaseCount);

            boardT.DoWork(new Hour(723));
            Assert.AreEqual(1, boardT.FinishedCaseCount);
        }



        [TestMethod()]
        public void CaseLogTest()
        {
            AppealCase appealCase = new AppealCase();
            Hour hour = new Hour(0);
            boardT.ProcessNewCase(appealCase, hour);
            //Assert.AreEqual(hour, appealCase.Log.Allocated);
            
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.SummonsEnqueuedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.SummonsStartedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.SummonsFinishedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.SummonsEnqueuedSecondMember);

            hour = new Hour(1);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.SummonsStartedSecondMember);
            Assert.AreEqual(hour, appealCase.Log.SummonsFinishedSecondMember);
            Assert.AreEqual(hour, appealCase.Log.SummonsEnqueuedChair);

            hour = new Hour(2);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.SummonsStartedChair);
            Assert.AreEqual(hour, appealCase.Log.SummonsFinishedChair);
            Assert.AreEqual(hour, appealCase.Log.OPEnqueuedChair);

            hour = new Hour(711);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.OPStartedChair);
            Assert.AreEqual(hour, appealCase.Log.OPStartedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.OPStartedSecondMember);
            
            for (int i = 712; i < 719; i++)
            {
                hour = new Hour(i);
                boardT.DoWork(hour);
            }

            hour = new Hour(719);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.OPFinishedChair);
            Assert.AreEqual(hour, appealCase.Log.OPFinishedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.OPFinishedSecondMember);
            Assert.AreEqual(hour, appealCase.Log.DecisionEnqueuedRapporteur);


            hour = new Hour(720);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.DecisionStartedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.DecisionFinishedRapporteur);
            Assert.AreEqual(hour, appealCase.Log.DecisionEnqueuedSecondMember);

            hour = new Hour(721);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.DecisionStartedSecondMember);
            Assert.AreEqual(hour, appealCase.Log.DecisionFinishedSecondMember);
            Assert.AreEqual(hour, appealCase.Log.DecisionEnqueuedChair);

            hour = new Hour(722);
            boardT.DoWork(hour);
            Assert.AreEqual(hour, appealCase.Log.DecisionStartedChair);
            Assert.AreEqual(hour, appealCase.Log.DecisionFinishedChair);
            Assert.AreEqual(hour, appealCase.Log.Finished);
        }

    }

}