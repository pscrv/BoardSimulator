using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{
    [TestClass()]
    public class CaseLoggingTests
    {
        Member chair ;
        Member rapporteur;
        Member other;
        Board board;
        MemberParameters parameters;
        MemberParameterCollection parameterCollection;
        AppealCase appealCase;
        AllocatedCase allocatedCase;

        BoardQueue boardQueues;
        IncomingCaseQueue incoming;
        CirculationQueue circulation;
        OPSchedule opSchedule;
        FinishedCaseList finished;


        [TestInitialize]
        public void Initialise()
        {
            boardQueues = new BoardQueue();
            incoming = new IncomingCaseQueue();
            circulation = new CirculationQueue();
            opSchedule = new OPSchedule(circulation);
            finished = new FinishedCaseList();

            parameters = new MemberParameters(2, 1, 2);
            parameterCollection = new MemberParameterCollection(parameters, parameters, parameters);
            chair =  new Member(parameterCollection);
            rapporteur = new Member(parameterCollection);
            other = new Member(parameterCollection);
            board = new Board(
                chair, 
                ChairType.Technical, 
                new List<Member> { rapporteur }, 
                new List<Member> { other },
                incoming,
                opSchedule);

            appealCase = new AppealCase();
            allocatedCase = board.ProcessNewCase(appealCase, new Hour(0));
        }



        [TestMethod()]
        public void EnqueueSummons()
        {
            Hour hour = new Hour(0);
            allocatedCase.EnqueueForWork(hour);

            Assert.AreEqual(hour, allocatedCase.Record.Creation);
            Assert.AreEqual(hour, allocatedCase.Record.Allocation);
            Assert.AreEqual(hour, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.IsNull(allocatedCase.Record.OtherMemberSummons.Enqueue);
            Assert.IsNull(allocatedCase.Record.ChairSummons.Enqueue);
        }

        [TestMethod()]
        public void RapporteurWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);

            board.DoWork(hour0);
            board.DoWork(hour1);

            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Start);
            Assert.AreEqual(hour1, allocatedCase.Record.RapporteurSummons.Finish);
            Assert.AreEqual(0, board.MemberQueueCount(rapporteur));
            Assert.AreEqual(1, board.CirculationQueueCount());
        }

        [TestMethod()]
        public void OtherMemberWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2= new Hour(2);
            Hour hour3 = new Hour(3);
            
            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);


            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Start, "Start");
            Assert.AreEqual(hour3, allocatedCase.Record.OtherMemberSummons.Finish, "Finish");
            Assert.AreEqual(0, board.MemberQueueCount(other));
            Assert.AreEqual(1, board.CirculationQueueCount());
        }

        [TestMethod()]
        public void ChairWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);

            
            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);

            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Start, "Start");
            Assert.AreEqual(hour5, allocatedCase.Record.ChairSummons.Finish, "Finish");
            Assert.AreEqual(0, board.MemberQueueCount(chair));
            Assert.AreEqual(1, board.CirculationQueueCount());
        }

        [TestMethod()]
        public void EnequeueOP()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Hour hour6 = new Hour(6);

            
            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);
            board.DoWork(hour6);

            Assert.AreEqual(hour6, allocatedCase.Record.OP.Enqueue, "Enqueue");
            Assert.AreEqual(0, board.CirculationQueueCount());
            Assert.AreEqual(3, opSchedule.Count);
        }
        

        [TestMethod()]
        public void EnequeueDecision()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Hour hour6 = new Hour(6);
            Hour hour7 = new Hour(7);

            
            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);
            board.DoWork(hour6);      
            _skipOP(hour7);
            board.DoWork(hour7);

            Assert.AreEqual(hour7, allocatedCase.Record.RapporteurDecision.Enqueue, "Enqueue");
        }


        [TestMethod()]
        public void RapporteurDecisionWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Hour hour6 = new Hour(6);
            Hour hour7 = new Hour(7);
            Hour hour8 = new Hour(8);
            Hour hour9 = new Hour(9);


            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);
            board.DoWork(hour6);
            _skipOP(hour7);
            board.DoWork(hour7);
            board.DoWork(hour8);
            board.DoWork(hour9);

            Assert.AreEqual(hour7, allocatedCase.Record.RapporteurDecision.Start, "Start");
            Assert.AreEqual(hour8, allocatedCase.Record.RapporteurDecision.Finish, "Finish");
        }


        [TestMethod()]
        public void OtherMemberDecisionWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Hour hour6 = new Hour(6);
            Hour hour7 = new Hour(7);
            Hour hour8 = new Hour(8);
            Hour hour9 = new Hour(9);
            Hour hour10 = new Hour(10);
            Hour hour11 = new Hour(11);


            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);
            board.DoWork(hour6);
            _skipOP(hour7);
            board.DoWork(hour7);
            board.DoWork(hour8);
            board.DoWork(hour9);
            board.DoWork(hour10);

            Assert.AreEqual(hour9, allocatedCase.Record.OtherMemberDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour9, allocatedCase.Record.OtherMemberDecision.Start, "Start");
            Assert.AreEqual(hour10, allocatedCase.Record.OtherMemberDecision.Finish, "Finish");
        }

        [TestMethod()]
        public void ChairDecisionWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2 = new Hour(2);
            Hour hour3 = new Hour(3);
            Hour hour4 = new Hour(4);
            Hour hour5 = new Hour(5);
            Hour hour6 = new Hour(6);
            Hour hour7 = new Hour(7);
            Hour hour8 = new Hour(8);
            Hour hour9 = new Hour(9);
            Hour hour10 = new Hour(10);
            Hour hour11 = new Hour(11);
            Hour hour12 = new Hour(12);
            Hour hour13 = new Hour(13);


            board.DoWork(hour0);
            board.DoWork(hour1);
            board.DoWork(hour2);
            board.DoWork(hour3);
            board.DoWork(hour4);
            board.DoWork(hour5);
            board.DoWork(hour6);
            _skipOP(hour7);
            board.DoWork(hour7);
            board.DoWork(hour8);
            board.DoWork(hour9);
            board.DoWork(hour10);
            board.DoWork(hour11);
            board.DoWork(hour12);

            Assert.AreEqual(hour11, allocatedCase.Record.ChairDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour11, allocatedCase.Record.ChairDecision.Start, "Start");
            Assert.AreEqual(hour12, allocatedCase.Record.ChairDecision.Finish, "Finish");
        }






        private void _skipOP(Hour hour)
        {
            allocatedCase.Record.SetOPStart(hour);
            allocatedCase.Record.SetOPFinished(hour);
            board.AddToCirculationQueue(allocatedCase, hour);
           
        }
        
    }
}