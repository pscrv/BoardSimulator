using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulator
{
    internal abstract class BoardBase
    {
        #region fields
        internal readonly Member Chair;
        internal readonly ReadOnlyCollection<Member> Technicals;
        internal readonly ReadOnlyCollection<Member> Legals;
        #endregion


        #region abstract
        protected abstract bool _configurationIsInvalid(List<Member> technicals, List<Member> legals);

        internal abstract List<AllocatedCase> FinishedCases { get; }
        
        internal abstract AllocatedCase ProcessNewCase(AppealCase appealCase, Hour currentHour);
        internal abstract BoardReport DoWork(Hour currentHour);
        internal abstract void AddToCirculationQueue(AllocatedCase allocatedCase, Hour currentHour);

        internal abstract int MemberQueueCount(Member member);
        internal abstract int CirculationQueueCount();
        internal abstract int OPScheduleCount();
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
}


    internal abstract class Board : BoardBase
    {
        #region static factory methods
        internal static Board MakeTechnicalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
        {
            return new TechnicalBoard(chair, technicals, legals, registrar, chairChooser);
        }

        internal static Board MakeLegalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
        {
            return new LegalBoard(chair, technicals, legals, registrar, chairChooser);
        }

        #endregion


        #region abstract
        protected abstract bool _boardChairMustBeCaseChair();
        protected abstract bool _chairIsTechnical();
        #endregion



        #region fields
        private Registrar _registrar;
        private ChairChooser _chairChooser;
        private Dictionary<Member, int> _allocationCount;
        #endregion


        #region private properties
        private IEnumerable<Member> _members
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


        #region BoardBase overrides
        internal override AllocatedCase ProcessNewCase(AppealCase appealCase, Hour currentHour)
        {
            AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
            _registrar.ProcessIncomingCase(currentHour, allocatedCase);
            return allocatedCase;
        }

        internal override List<AllocatedCase> FinishedCases
        {
            get { return _registrar.FinishedCases; }
        }
        
        internal override BoardReport DoWork(Hour currentHour)
        {
            BoardReport boardReport = new BoardReport(_members);

            _registrar.DoWork(currentHour);

            foreach (Member member in _members)
            {
                //working
                AllocatedCase currentCase = _registrar.GetCurrentCase(currentHour, member);
                WorkReport report = _memberWork(currentHour, currentCase, member);
                boardReport.Add(member, report);

                //boardReport.Add(member, _memberWork(currentHour, member));
            }

            return boardReport;
        }

        internal override void AddToCirculationQueue(AllocatedCase allocatedCase, Hour currentHour)
        {
            _registrar.AddToCirculation(currentHour, allocatedCase);
        }

        internal override int MemberQueueCount(Member member)
        {
            return _registrar.MemberQueueCount(member);
        }

        internal override int CirculationQueueCount()
        {
            return _registrar.CirculationQueueCount();
        }

        internal override int OPScheduleCount()
        {
            return _registrar.OPScheduleCount();
        }           
        #endregion


        #region construction
        protected Board(
            Member chair,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
            : base(chair, technicals, legals)
        {
            _registrar = registrar;
            _chairChooser = chairChooser;

            _allocationCount = new Dictionary<Member, int>();
            foreach (Member member in _members)
            {
                _allocationCount[member] = 0;
                _registrar.RegisterMember(member);
            }
        }
        #endregion

        

        #region private methods
        private WorkReport _memberWork(Hour currentHour, Member member)
        {
            WorkReport report;
            AllocatedCase currentCase = _registrar.GetCurrentCase(currentHour, member);

            report = member.Work(currentHour, currentCase);
            if (report.State == WorkState.Finished)
            {
                _registrar.ProcessFinishedWork(currentHour, currentCase, member);
            }

            return report;
        }

        //working
        private WorkReport _memberWork(Hour currentHour, AllocatedCase currentCase, Member member)
        {
            WorkReport report;

            report = member.Work(currentHour, currentCase);
            if (report.State == WorkState.Finished)
            {
                _registrar.ProcessFinishedWork(currentHour, currentCase, member);
            }

            return report;
        }


        private AllocatedCase _allocateCase(AppealCase appealCase, Hour currentHour)
        {
            Member chair;
            Member rapporteur;
            Member other;

            chair = _allocateChair();
            rapporteur = _allocateRapporteur(chair);
            other = _allocateOtherMember(chair, rapporteur);

            return new AllocatedCase(
                appealCase,
                new CaseBoard(chair, rapporteur, other, _registrar),
                currentHour);
        }
        
        private Member _allocateChair()
        {
            return _boardChairMustBeCaseChair() ? Chair : _chairChooser.ChooseChair();
        }

        private Member _allocateRapporteur(Member chair)
        {
            Member rapporteur = _getMemberWithFewestAllocations(Technicals.Where(x => x != chair));
            _allocationCount[rapporteur]++;
            return rapporteur;
        }

        private Member _allocateOtherMember(Member chair, Member rapporteur)
        {
            ReadOnlyCollection<Member> choices = _isTechnicalMember(chair) ? Legals : Technicals;
            Member other = _getMemberWithFewestAllocations(choices.Where(x => x != chair && x != rapporteur));
            _allocationCount[other]++;
            return other;
        }

        private Member _getMemberWithFewestAllocations(IEnumerable<Member> members)
        {
            return members.Aggregate(
                (currentMin, m) => _allocationCount[m] < _allocationCount[currentMin] ? m : currentMin);
        }

        private bool _isTechnicalMember(Member member)
        {
            return (member == Chair) ?
                _chairIsTechnical() : Technicals.Contains(member);
        }
        #endregion 
    }

    


    internal class TechnicalBoard : Board
    {

        internal TechnicalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
            : base(chair, technicals, legals, registrar, chairChooser)
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
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
            : base(chair, technicals, legals, registrar, chairChooser)
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