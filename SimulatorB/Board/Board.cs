using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimulatorB
{
    internal abstract class Board
    {
        #region fields
        internal readonly Member Chair;
        internal readonly ReadOnlyCollection<Member> Technicals;
        internal readonly ReadOnlyCollection<Member> Legals;
        #endregion


        #region abstract properties
        internal abstract int CirculatedSummonsCount { get; }
        internal abstract int CirculatedDecisionsCount { get; }
        internal abstract int PendingOPCount { get; }
        internal abstract int RunningOPCount { get; }
        internal abstract int FinishedCaseCount { get; }
        internal abstract List<CompletedCaseReport> FinishedCases { get; }
        #endregion


        #region abstract methods
        protected abstract bool _configurationIsInvalid(List<Member> technicals, List<Member> legals);

        internal abstract void ProcessNewCase(AppealCase appealCase, Hour currentHour);
        internal abstract BoardReport DoWork(Hour currentHour);
        #endregion



        #region construction
        protected Board(
            Member chair, 
            List<Member> technicals, 
            List<Member> legals)
        {
            if (_configurationIsInvalid(technicals, legals))
                throw new ArgumentException("Invalid board configuration.");

            Chair = chair;
            Technicals = technicals.AsReadOnly();
            Legals = legals.AsReadOnly();
        }
        #endregion


        #region protected properties
        protected IEnumerable<Member> _members
        {
            get
            {
                yield return Chair;
                foreach (Member tm in Technicals)
                    yield return tm;
                foreach (Member lm in Legals)
                    yield return lm;

            }
        }
        #endregion

    }


    internal abstract class BoardCommon : Board
    {

        #region abstract
        protected abstract bool _boardChairMustBeCaseChair();
        protected abstract IEnumerable<Member> _getSecondMemberChoices();
        #endregion


        #region fields
        protected Registrar _registrar;
        protected Allocator _allocator;
        
        private Dictionary<AppealCase, IEnumerable<CaseWorker>> _allocations;
        #endregion


        #region construction
        protected BoardCommon(
            Member chair,
            List<Member> technicals,
            List<Member> legals
            )
            : base(chair, technicals, legals)
        {
            _registrar = new BasicRegistrar(_members);
            _allocator = new Allocator(_members);


            _allocations = new Dictionary<AppealCase, IEnumerable<CaseWorker>>();
        }
        #endregion


        #region property overrides
        internal override int CirculatedSummonsCount => _registrar.CirculatedSummonsCount;
        internal override int CirculatedDecisionsCount => _registrar.CirculatedDecisionsCount;
        internal override int PendingOPCount => _registrar.PendingOPCount;
        internal override int RunningOPCount => _registrar.RunningOPCount;
        internal override int FinishedCaseCount => _registrar.FinishedCaseCount;
        internal override List<CompletedCaseReport> FinishedCases => _registrar.FinishedCases;
        #endregion


        #region method overrides
        internal override void ProcessNewCase(AppealCase appealCase, Hour currentHour)
        {
            CaseBoard allocation = 
                _allocator.GetAllocation(
                    appealCase, 
                    Chair, 
                    Technicals, 
                    _getSecondMemberChoices());
            
            WorkCase summonsCase = new SummonsCase(appealCase, allocation);
            _registrar.ProcessNewSummons(summonsCase);
            _registrar.CirculateCases(currentHour);
        }

        internal override BoardReport DoWork(Hour currentHour)
        {
            var report = _passWorkToMembers(currentHour);
            _registrar.UpdateQueuesAndCirculate(currentHour);
            return report;
        }
        #endregion


        private BoardReport _passWorkToMembers(Hour currentHour)
        {
            BoardReport report = new BoardReport(_members);

            WorkCase workCase;
            WorkReport workReport;
            foreach (Member member in _members)
            {
                workCase = _registrar.GetMemberWork(currentHour, member);
                workReport = (workCase == null)
                    ? new NullWorkReport()
                    : workCase.Work(currentHour, member);
                report.Add(member, workReport);
            }

            return report;
        }


    }


    internal class TechnicalBoard : BoardCommon
    {

        internal TechnicalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals
            )
            : base(chair, technicals, legals)
        { }


        #region BoardBase overrides
        protected override bool _configurationIsInvalid(List<Member> technicals, List<Member> legals)
        {
            return technicals.Count < 1 || legals.Count < 1;
        }

        protected override bool _boardChairMustBeCaseChair()
        {
            return Technicals.Count < 2;
        }

        protected override IEnumerable<Member> _getSecondMemberChoices()
        {
            return Legals;
        }
        #endregion
    }


    internal class LegalBoard : BoardCommon
    {

        internal LegalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals
            )
            : base(chair, technicals, legals)
        { }


        #region overrides
        protected override bool _configurationIsInvalid(List<Member> technicals, List<Member> legals)
        {
            return technicals.Count < 2;
        }

        protected override bool _boardChairMustBeCaseChair()
        {
            return Legals.Count < 1;
        }

        protected override IEnumerable<Member> _getSecondMemberChoices()
        {
            return Technicals;
        }
        #endregion
    }

}