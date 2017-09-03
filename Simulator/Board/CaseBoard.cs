using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class CaseBoard
    {
        #region fields and properties
        private Dictionary<WorkType, Queue<CaseWorker>> _workerQueues;

        private Registrar _registrar;

        internal readonly CaseWorker Chair;
        internal readonly CaseWorker Rapporteur;
        internal readonly CaseWorker OtherMember;
        
        internal Queue<CaseWorker> SummonsQueue { get { return _workerQueues[WorkType.Summons]; } }
        internal Queue<CaseWorker> DecisionQueue { get { return _workerQueues[WorkType.Decision]; } }


        internal IEnumerable<CaseWorker> MembersAsCaseWorkers
        {
            get
            {
                yield return Rapporteur;
                yield return OtherMember;
                yield return Chair;
            }
        }

        internal IEnumerable<Member> Members { get => MembersAsCaseWorkers.Select(x => x.Member); }
        #endregion


        #region construction
        internal CaseBoard(
            Member ch, 
            Member rp, 
            Member om, 
            Registrar registrar)
        {
            Chair = new CaseWorker(ch, WorkerRole.Chair);
            Rapporteur = new CaseWorker(rp, WorkerRole.Rapporteur);
            OtherMember = new CaseWorker(om, WorkerRole.OtherMember);

            _workerQueues = new Dictionary<WorkType, Queue<CaseWorker>>
            {
                [WorkType.Summons] = _makeQueue(),
                [WorkType.Decision] = _makeQueue()
            };
            _registrar = registrar;
        }


        private Queue<CaseWorker> _makeQueue()
        {
            Queue<CaseWorker> queue = new Queue<CaseWorker>();
            queue.Enqueue(Rapporteur);
            queue.Enqueue(OtherMember);
            queue.Enqueue(Chair);
            return queue;
        }
        #endregion



        internal Member GetMemberByRole(WorkerRole role)
        {
            switch (role)
            {
                case WorkerRole.Rapporteur:
                    return Rapporteur.Member;
                case WorkerRole.Chair:
                    return Chair.Member;
                case WorkerRole.OtherMember:
                    return OtherMember.Member;
                default:
                    throw new InvalidOperationException("CaseBoard.GetMemberByRole: something very odd happened.");
            }
        }

        internal bool IsInCaseBoard(Member member)
        {
            return
                member == Chair.Member
                || member == Rapporteur.Member
                || member == OtherMember.Member;
        }

        internal CaseWorker GetMemberAsCaseWorker(Member member)
        {
            if (member == Chair.Member)
                return Chair;
            if (member == Rapporteur.Member)
                return Rapporteur;
            if (member == OtherMember.Member)
                return OtherMember;
            return null;
        }

        internal WorkerRole GetRole(Member member)
        {
            if (member == Chair.Member)
                return WorkerRole.Chair;
            if (member == Rapporteur.Member)
                return WorkerRole.Rapporteur;
            if (member == OtherMember.Member)
                return WorkerRole.OtherMember;
            return WorkerRole.None;
        }

        internal WorkerRole EnqueueForNextWorker(Hour currentHour, AllocatedCase allocatedCase)
        {
            CaseWorker nextWorker = null;

            if (_workerQueues[WorkType.Summons].Count > 0)
                nextWorker = _workerQueues[WorkType.Summons].Dequeue();
            else if (_workerQueues[WorkType.Decision].Count > 0)
                nextWorker = _workerQueues[WorkType.Decision].Dequeue();

            if (nextWorker == null)
                return WorkerRole.None;
            
            _registrar.EnqueueForMember(currentHour, nextWorker, allocatedCase);
            return nextWorker.Role;
        }


        internal void ScheduleOP(Hour currentHour, AllocatedCase allocatedCase)
        {
            _registrar.ScheduleOP(currentHour, allocatedCase);
        }


        internal int GetLongestOPPreparationHours()
        {
            return Math.Max(
                Chair.HoursOPPreparation,
                Math.Max(
                    Rapporteur.HoursOPPreparation,
                    OtherMember.HoursOPPreparation));
        }

        internal int GetShortestOPPreparationHours()
        {
            return Math.Min(
                Chair.HoursOPPreparation,
                Math.Min(
                    Rapporteur.HoursOPPreparation,
                    OtherMember.HoursOPPreparation));
        }
    }
}