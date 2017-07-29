using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulator.Tests
{
    [TestClass()]
    public class CaseLoggingTests
    {
        Member chair ;
        Member rapporteur;
        Member other;
        MemberParameters parameters;
        MemberParameterCollection parameterCollection;
        AppealCase appealCase;
        AllocatedCase allocatedCase;
        CaseBoard caseBoard;

        BoardQueue boardQueues;
        CirculationQueue circulation;
        OPSchedule opSchedule;



        [TestInitialize]
        public void Initialise()
        {
            boardQueues = new BoardQueue();
            circulation = new CirculationQueue();
            opSchedule = new OPSchedule(circulation);

            parameters = new MemberParameters(2, 1, 2);
            parameterCollection = new MemberParameterCollection(parameters, parameters, parameters);
            chair =  new Member(parameterCollection, boardQueues, circulation);
            rapporteur = new Member(parameterCollection, boardQueues, circulation);
            other = new Member(parameterCollection, boardQueues, circulation);
            appealCase = new AppealCase();

            caseBoard = new CaseBoard(chair, rapporteur, other, boardQueues);
            allocatedCase = new AllocatedCase(appealCase, caseBoard, new Hour(0), opSchedule);
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
            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);

            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Enqueue);
            Assert.AreEqual(hour0, allocatedCase.Record.RapporteurSummons.Start);
            Assert.AreEqual(hour1, allocatedCase.Record.RapporteurSummons.Finish);
            Assert.AreEqual(0, boardQueues.Count(rapporteur));
            Assert.AreEqual(1, circulation.Count);
        }

        [TestMethod()]
        public void OtherMemberWork()
        {
            Hour hour0 = new Hour(0);
            Hour hour1 = new Hour(1);
            Hour hour2= new Hour(2);
            Hour hour3 = new Hour(3);

            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);


            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour2, allocatedCase.Record.OtherMemberSummons.Start, "Start");
            Assert.AreEqual(hour3, allocatedCase.Record.OtherMemberSummons.Finish, "Finish");
            Assert.AreEqual(0, boardQueues.Count(other));
            Assert.AreEqual(1, circulation.Count);
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

            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            
            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Enqueue, "Enqueue");
            Assert.AreEqual(hour4, allocatedCase.Record.ChairSummons.Start, "Start");
            Assert.AreEqual(hour5, allocatedCase.Record.ChairSummons.Finish, "Finish");
            Assert.AreEqual(0, boardQueues.Count(chair));
            Assert.AreEqual(1, circulation.Count);
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
            

            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            _incrementTimeAndCirculateCases(hour5);

            Assert.AreEqual(hour6, allocatedCase.Record.OP.Enqueue, "Enqueue");
            Assert.AreEqual(0, circulation.Count);
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


            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            _incrementTimeAndCirculateCases(hour5);
            _skipOP(hour7);

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


            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            _incrementTimeAndCirculateCases(hour5);
            _skipOP(hour7);
            _incrementTimeAndCirculateCases(hour7);
            _doRapporteurWork(hour8);

            Assert.AreEqual(hour8, allocatedCase.Record.RapporteurDecision.Start, "Start");
            Assert.AreEqual(hour9, allocatedCase.Record.RapporteurDecision.Finish, "Finish");
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


            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            _incrementTimeAndCirculateCases(hour5);
            _skipOP(hour7);
            _incrementTimeAndCirculateCases(hour7);
            _doRapporteurWork(hour8);
            _incrementTimeAndCirculateCases(hour9);
            _doOtherMemberWork(hour10);

            Assert.AreEqual(hour10, allocatedCase.Record.OtherMemberDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour10, allocatedCase.Record.OtherMemberDecision.Start, "Start");
            Assert.AreEqual(hour11, allocatedCase.Record.OtherMemberDecision.Finish, "Finish");
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


            allocatedCase.EnqueueForWork(hour0);
            _doRapporteurWork(hour0);
            _incrementTimeAndCirculateCases(hour1);
            _doOtherMemberWork(hour2);
            _incrementTimeAndCirculateCases(hour3);
            _doChairWork(hour4);
            _incrementTimeAndCirculateCases(hour5);
            _skipOP(hour7);
            _incrementTimeAndCirculateCases(hour7);
            _doRapporteurWork(hour8);
            _incrementTimeAndCirculateCases(hour9);
            _doOtherMemberWork(hour10);
            _incrementTimeAndCirculateCases(hour11);
            _doChairWork(hour12);

            Assert.AreEqual(hour12, allocatedCase.Record.ChairDecision.Enqueue, "Enqueue");
            Assert.AreEqual(hour12, allocatedCase.Record.ChairDecision.Start, "Start");
            Assert.AreEqual(hour13, allocatedCase.Record.ChairDecision.Finish, "Finish");
        }


        private void _skipOP(Hour hour)
        {
            foreach (AllocatedCase ac in opSchedule.ScheduledCases)
            {
                ac.Record.SetOPStart(hour);
                ac.Record.SetOPFinished(hour);
                ac.EnqueueForWork(hour);
            }
        }
        

        private void _incrementTimeAndCirculateCases(Hour hour)
        {
            circulation.EnqueueForNextStage(hour.AddHours(1));
        }
        


        private void _doRapporteurWork(Hour hour)
        {
            allocatedCase.Board.Rapporteur.Member.Work(hour);
            allocatedCase.Board.Rapporteur.Member.Work(hour.AddHours(1));
        }
        

        private void _doOtherMemberWork(Hour hour)
        {
            allocatedCase.Board.OtherMember.Member.Work(hour);
            allocatedCase.Board.OtherMember.Member.Work(hour.AddHours(1));
        }
        

        private void _doChairWork(Hour hour)
        {
            allocatedCase.Board.Chair.Member.Work(hour);
            allocatedCase.Board.Chair.Member.Work(hour.AddHours(1));
        }
        
    }
}