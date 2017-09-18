﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimulatorB
{
    internal abstract class BoardBase
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

        internal abstract void ProcessNewCase(AppealCase appealCase);
        internal abstract void DoWork(Hour currentHour);
        #endregion



        #region construction
        protected BoardBase(
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


    internal abstract class Board : BoardBase
    {

        #region abstract
        protected abstract bool _boardChairMustBeCaseChair();
        protected abstract bool _chairIsTechnical();
        #endregion


        #region fields
        protected Registrar _registrar;
        protected Allocator _allocator;
        
        private Dictionary<AppealCase, IEnumerable<CaseWorker>> _allocations;
        #endregion


        #region construction
        protected Board(
            Member chair,
            List<Member> technicals,
            List<Member> legals
            )
            : base(chair, technicals, legals)
        {
            _registrar = new Registrar(_members);
            _allocator = new Allocator(_members);


            _allocations = new Dictionary<AppealCase, IEnumerable<CaseWorker>>();
        }
        #endregion


        #region BoardBase property overrides
        internal override int CirculatingSummonsCount => _registrar.CirculatingSummonsCount;
        internal override int CirculatingDecisionsCount => _registrar.CirculatingDecisionsCount;
        internal override int PendingOPCount => _registrar.PendingOPCount;
        internal override int RunningOPCount => _registrar.RunningOPCount;
        internal override int FinishedCaseCount => _registrar.FinishedCaseCount;
        #endregion


        #region BoardBase method overrides
        internal override void ProcessNewCase(AppealCase appealCase)
        {
            var allocation = 
                _allocator.GetAllocation(
                    appealCase, 
                    Chair, 
                    _getRapporteurChoices(), 
                    _getSecondMemberChoices());

            var summonsCase = new SummonsCase(appealCase, allocation);
            _registrar.AddToSummonsCirculation(summonsCase);
        }


        internal override void DoWork(Hour currentHour)
        {
            _registrar.DoWork(currentHour);
            
            WorkCase workCase;
            foreach (Member member in _members)
            {
                if (_registrar.MemberHasOP(currentHour, member))
                    continue;
                
                workCase = _registrar.GetMemberWork(member);
                workCase?.WorkAndPassToRegistrar(currentHour, _registrar);
            }
        }
        #endregion


        #region private methods
        private IEnumerable<Member> _getRapporteurChoices()
        {
            return Technicals;
        }

        private IEnumerable<Member> _getSecondMemberChoices()
        {
            return _chairIsTechnical() ? Legals : Technicals;
        }        
        #endregion



    }




    internal class TechnicalBoard : Board
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

        protected override bool _chairIsTechnical()
        {
            return true;
        }
        #endregion
    }


    internal class LegalBoard : Board
    {

        internal LegalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals
            )
            : base(chair, technicals, legals)
        { }


        #region BoardBase overrides
        protected override bool _configurationIsInvalid(List<Member> technicals, List<Member> legals)
        {
            return technicals.Count < 2;
        }

        protected override bool _boardChairMustBeCaseChair()
        {
            return Legals.Count < 1;
        }

        protected override bool _chairIsTechnical()
        {
            return false;
        }
        #endregion
    }

}