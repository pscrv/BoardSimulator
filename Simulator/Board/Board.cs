using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulator
{
    internal class Board
    {
        #region fields and properties
        private Member _chair;
        private ChairType _chairType;
        private List<Member> _technicals;
        private List<Member> _legals;
        private Registrar _registrar;

        private Dictionary<Member, int> _allocationCount;

        private IEnumerable<Member> _members
        {
            get
            {
                yield return _chair;
                foreach (Member tm in _technicals)
                    yield return tm;
                foreach (Member lm in _legals)
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
            List<Member> legals)
        {
            _chair = chair;
            _chairType = chairType;
            _technicals = technicals;
            _legals = legals;

            _registrar = new Registrar();

            _allocationCount = new Dictionary<Member, int>();
            foreach (Member member in _members)
            {
                _allocationCount[member] = 0;
                _registrar.RegisterMember(member);
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
            // TODO: make this choose (sometimes? when?) a different chair
            // than the board chair

            // TODO: do better than just counting allocations

            Member chair;
            Member rapporteur;
            Member other = null;

            chair = _chair;
            _allocationCount[chair]++;
            rapporteur = _getMemberWithFewestAllocations(_technicals);
            _allocationCount[rapporteur]++;
            
            switch (_chairType)
            {
                case ChairType.Technical:
                    other = _getMemberWithFewestAllocations(_legals);
                    break;
                case ChairType.Legal:
                    List<Member> choices = _technicals.Where(x => x != rapporteur).ToList();
                    other = _getMemberWithFewestAllocations(choices);
                    break;
            }
            _allocationCount[other]++;
            

            return new AllocatedCase(
                appealCase, 
                new CaseBoard(chair, rapporteur, other, _registrar), 
                currentHour);
        }


        private Member _getMemberWithFewestAllocations(List<Member> members)
        {
            return members.Aggregate(
                (currentMin, m) => _allocationCount[m] < _allocationCount[currentMin] ? m : currentMin );            
        }
    }
}