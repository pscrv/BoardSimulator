using System;
using System.Collections.Generic;

namespace Simulator
{
    internal static class WorkQueues
    {
        private static Dictionary<Member, MemberWorkQueue> _memberQueues = new Dictionary<Member, MemberWorkQueue>();
        private static CaseQueue _circulatingCases = new CaseQueue();
        private static CaseQueue _opCases = new CaseQueue();


        #region MemberQueues
        internal static void RegisterMember(Member member)
        {
            if (_memberQueues.ContainsKey(member))
                throw new InvalidOperationException("Member is already registered.");

            _memberQueues[member] = new MemberWorkQueue();
        }


        internal static void EnqueueForMember(Member member, WorkerRole role, AllocatedCase allocatedCase)
        {
            _checkMemberIsRegistered(member);
            _memberQueues[member].Enqueue(allocatedCase, role);
        }

        internal static AllocatedCase DequeueForMember(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Dequeue();
        }

        internal static AllocatedCase PeekForMember(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Peek();
        }

        internal static int CountForMember(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Count;
        }


        private static void _checkMemberIsRegistered(Member member)
        {
            if (!_memberQueues.ContainsKey(member))
                throw new InvalidOperationException("Member is not registered.");
        }
        #endregion




        #region Circulation
        internal static void EnqueueForCirculation(AllocatedCase allocatedCase)
        {
            _circulatingCases.Enqueue(allocatedCase);
        }

        internal static AllocatedCase DequeueFromCirculation()
        {
            return _circulatingCases.Dequeue();
        }

        internal static IEnumerable<AllocatedCase> CirculatingCases
        {
            get
            {
                while (_circulatingCases.Count > 0)
                {
                    yield return _circulatingCases.Dequeue();
                }
            }
        }

        internal static int CiculatingCaseCount { get { return _circulatingCases.Count; } }

        #endregion


        #region OP
        internal static void EnqueueForOP(AllocatedCase allocatedCase)
        {
            _opCases.Enqueue(allocatedCase);
        }

        internal static AllocatedCase DequeueFromOP()
        {
            return _opCases.Dequeue();
        }

        internal static IEnumerable<AllocatedCase> OPCases
        {
            get
            {
                while (_opCases.Count > 0)
                {
                    yield return _opCases.Dequeue();
                }
            }
        }

        internal static int OPCaseCount { get { return _opCases.Count; } }
        #endregion
    }
}