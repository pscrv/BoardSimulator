using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulator
{
    internal abstract class BoardBase
    {
        #region temporary stuff
        protected ChairType _chairType;

        public BoardBase(
            Member chair,
            ChairType type,
            List<Member> technicals,
            List<Member> legals)
        {
            _chairType = type;
            if (_configurationIsInvalid(technicals, legals))
                throw new ArgumentException("Invalid board configuration.");

            Chair = chair;
            Technicals = technicals.AsReadOnly();
            Legals = legals.AsReadOnly();

        }
        #endregion


        #region fields
        internal readonly Member Chair;
        internal readonly ReadOnlyCollection<Member> Technicals;
        internal readonly ReadOnlyCollection<Member> Legals;
        #endregion


        #region abstract
        protected abstract bool _configurationIsInvalid(List<Member> technicals, List<Member> legals);

        internal abstract List<AllocatedCase> FinishedCases { get; }
        
        internal abstract AllocatedCase ProcessNewCase(AppealCase appealCase, Hour currentHour);
        internal abstract void ProcessNewCaseList(List<AppealCase> appealCases, Hour currentHour);
        
        internal abstract BoardReport DoWork(Hour currentHour);
        internal abstract void AddToCirculationQueue(AllocatedCase allocatedCase, Hour currentHour);
        internal abstract int MemberQueueCount(Member member);
        internal abstract int CirculationQueueCount();
        internal abstract int OPScheduleCount();
        #endregion



        #region construction
        public BoardBase(
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



    internal class Board : BoardBase
    {
        #region temporary
        public Board(
            Member chair,
            ChairType type,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
            : base(chair, type, technicals, legals)
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



        internal static Board MakeBoard(
            Member chair,
            ChairType type,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
        {
            Board newBoard = null;

            switch (type)
            {
                case ChairType.Technical:
                    newBoard =  new TechnicalBoard(chair, technicals, legals, registrar, chairChooser);
                    break;
                case ChairType.Legal:
                    newBoard = new LegalBoard(chair, technicals, legals, registrar, chairChooser);
                    break;
                default:
                    throw new ArgumentException("Invalid chair type.");
            }

            return newBoard;
        }



        #region fields
        protected Registrar _registrar;
        protected ChairChooser _chairChooser;
        protected Dictionary<Member, int> _allocationCount;
        #endregion


        #region private properties
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


        #region BoardBase overrides
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
                boardReport.Add(member, _memberWork(currentHour, member));
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



        internal override void ProcessNewCaseList(List<AppealCase> appealCases, Hour currentHour)
        {
            foreach (AppealCase appealCase in appealCases)
            {
                AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
                _registrar.ProcessIncomingCase(currentHour, allocatedCase);
            }
        }
        


        internal override AllocatedCase ProcessNewCase(AppealCase appealCase, Hour currentHour)
        {
            AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
            _registrar.ProcessIncomingCase(currentHour, allocatedCase);
            return allocatedCase;
        }
        #endregion


        #region construction
        public Board(
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
        protected override bool _configurationIsInvalid(List<Member> technicals, List<Member> legals)
        {
            try
            {
                _checkConfiguration(_chairType, technicals, legals);
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        private void _checkConfiguration(
            ChairType chairType,
            List<Member> technicals,
            List<Member> legals)
        {
            switch (chairType)
            {
                case ChairType.Technical:
                    if (technicals.Count < 1)
                        throw new ArgumentException("A technically-qualified chair requires at least one technically qualified member.");
                    if (legals.Count < 1)
                        throw new ArgumentException("A technically-qualified chair requires at least one legally qualified member.");
                    break;
                case ChairType.Legal:
                    if (technicals.Count < 2)
                        throw new ArgumentException("A technically-qualified chair requires at least two technically qualified members.");
                    break;
            }


        }



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



        private AllocatedCase _allocateCase(AppealCase appealCase, Hour currentHour)
        {
            // TODO: do better than just counting allocations?

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
            switch (_chairType)
            {
                case ChairType.Technical:
                    if (Technicals.Count < 2)
                        return Chair;
                    break;
                case ChairType.Legal:
                    if (Legals.Count < 1)
                        return Chair;
                    break;
            }

            return _chairChooser.ChooseChair();
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
                _chairType == ChairType.Technical : Technicals.Contains(member);
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
            : base(chair, ChairType.Technical, technicals, legals, registrar, chairChooser)
        { }
    }


    internal class LegalBoard : Board
    {

        internal LegalBoard(
            Member chair,
            List<Member> technicals,
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
            : base(chair, ChairType.Legal, technicals, legals, registrar, chairChooser)
        { }
    }

}