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
        internal abstract int CirculatingSummonsCount { get; }
        internal abstract int CirculatingDecisionsCount { get; }
        internal abstract int PendingOPCount { get; }
        internal abstract int RunningOPCount { get; }
        internal abstract int FinishedCaseCount { get; }
        #endregion


        #region abstract methods
        protected abstract bool _configurationIsInvalid(List<Member> technicals, List<Member> legals);

        internal abstract void ProcessNewCase(AppealCase appealCase, Hour currentHour);
        internal abstract void DoWork(Hour currentHour);
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
        internal override int CirculatingSummonsCount => _registrar.CirculatingSummonsCount;
        internal override int CirculatingDecisionsCount => _registrar.CirculatingDecisionsCount;
        internal override int PendingOPCount => _registrar.PendingOPCount;
        internal override int RunningOPCount => _registrar.RunningOPCount;
        internal override int FinishedCaseCount => _registrar.FinishedCaseCount;
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

        internal override void DoWork(Hour currentHour)
        {
            _passWorkToMembers(currentHour);
            _registrar.UpdateQueuesAndCirculate(currentHour);
        }
        #endregion


        private void _passWorkToMembers(Hour currentHour)
        {
            WorkCase workCase;
            foreach (Member member in _members)
            {
                workCase = _registrar.GetMemberWork(currentHour, member);
                workCase?.Work(currentHour, member);
            }
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