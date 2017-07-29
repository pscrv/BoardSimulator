using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class CaseBoard
    {
        #region fields and properties
        private Dictionary<WorkType, Queue<CaseWorker>> _workerQueues;

        internal readonly CaseWorker Chair;
        internal readonly CaseWorker Rapporteur;
        internal readonly CaseWorker OtherMember;
        
        internal Queue<CaseWorker> SummonsQueue { get { return _workerQueues[WorkType.Summons]; } }
        internal Queue<CaseWorker> DecisionQueue { get { return _workerQueues[WorkType.Decision]; } }


        internal IEnumerable<CaseWorker> Members
        {
            get
            {
                yield return Rapporteur;
                yield return OtherMember;
                yield return Chair;
            }
        }
        #endregion


        #region construction
        internal CaseBoard(Member ch, Member rp, Member om, BoardQueue boardQueues)
        {
            Chair = new CaseWorker(ch, WorkerRole.Chair, boardQueues);
            Rapporteur = new CaseWorker(rp, WorkerRole.Rapporteur, boardQueues);
            OtherMember = new CaseWorker(om, WorkerRole.OtherMember, boardQueues);

            _workerQueues = new Dictionary<WorkType, Queue<CaseWorker>>();
            _workerQueues[WorkType.Summons] = _makeQueue();
            _workerQueues[WorkType.Decision] = _makeQueue();
            
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

            nextWorker.Enqueue(currentHour, allocatedCase);
            return nextWorker.Role;
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