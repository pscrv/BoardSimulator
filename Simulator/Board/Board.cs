using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulator
{
    internal class Board
    {
        #region fields and properties
        internal readonly Member Chair;
        internal readonly ReadOnlyCollection<Member> Technicals;
        internal readonly ReadOnlyCollection<Member> Legals;

        private ChairType _chairType;
        private Registrar _registrar;
        private ChairChooser _chairChooser;

        private Dictionary<Member, int> _allocationCount;

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

        
        internal List<AllocatedCase> FinishedCases
        {
            get { return _registrar.FinishedCases; }
        }
        #endregion



        #region construction
        internal Board(
            Member chair, 
            ChairType chairType, 
            List<Member> technicals, 
            List<Member> legals,
            Registrar registrar,
            ChairChooser chairChooser)
        {
            _checkConfiguration(chairType, technicals, legals);

            Chair = chair;
            _chairType = chairType;
            Technicals = technicals.AsReadOnly();
            Legals = legals.AsReadOnly();            
            _registrar = registrar;
            _chairChooser = chairChooser;

            _allocationCount = new Dictionary<Member, int>();
            foreach (Member member in _members)
            {
                _allocationCount[member] = 0;
                _registrar.RegisterMember(member);
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
        #endregion



        internal BoardReport DoWork(Hour currentHour)
        {
            BoardReport boardReport = new BoardReport(_members);

            _registrar.DoWork(currentHour);

            foreach (Member member in _members)
            {
                boardReport.Add(member, _memberWork(currentHour, member));
            }

            return boardReport;
        }

        internal AllocatedCase ProcessNewCase(AppealCase appealCase, Hour currentHour)
        {
            AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
            _registrar.ProcessIncomingCase(currentHour, allocatedCase);
            return allocatedCase;
        }

        internal void ProcessNewCaseList(List<AppealCase> appealCases, Hour currentHour)
        {
            foreach (AppealCase appealCase in appealCases)
            {
                AllocatedCase allocatedCase = _allocateCase(appealCase, currentHour);
                _registrar.ProcessIncomingCase(currentHour, allocatedCase);
            }
        }

        internal void AddToCirculationQueue(AllocatedCase allocatedCase, Hour currentHour)
        {
            _registrar.AddToCirculation(currentHour, allocatedCase);
        }



        internal int MemberQueueCount(Member member)
        {
            return _registrar.MemberQueueCount(member);
        }

        internal int CirculationQueueCount()
        {
            return _registrar.CirculationQueueCount();
        }

        internal int OPScheduleCount()
        {
            return _registrar.OPScheduleCount();
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


        private bool _isTechnicalMember(Member member)
        {
            return (member == Chair) ? 
                _chairType == ChairType.Technical : Technicals.Contains(member);            
        }


        private Member _getMemberWithFewestAllocations(IEnumerable<Member> members)
        {
            return members.Aggregate(
                (currentMin, m) => _allocationCount[m] < _allocationCount[currentMin] ? m : currentMin );            
        }
    }
}