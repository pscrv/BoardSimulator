using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class BoardQueue
    {
        private Dictionary<Member, MemberWorkQueue> _memberQueues = new Dictionary<Member, MemberWorkQueue>();


        #region MemberQueues
        internal void Register(Member member)
        {
            if (_memberQueues.ContainsKey(member))
                throw new InvalidOperationException("Member is already registered.");

            _memberQueues[member] = new MemberWorkQueue();
        }



        internal void EnqueueForMember(Hour currentHour, Member member, WorkerRole role, AllocatedCase allocatedCase)
        {
            _checkMemberIsRegistered(member);
            _memberQueues[member].Enqueue(currentHour, allocatedCase, role);
        }

        internal AllocatedCase Dequeue(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Dequeue();
        }

        internal AllocatedCase Peek(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Peek();
        }

        internal int Count(Member member)
        {
            _checkMemberIsRegistered(member);
            return _memberQueues[member].Count;
        }

        internal void ClearAll()
        {
            foreach (Member member in _memberQueues.Keys)
            {
                while (_memberQueues[member].Count > 0)
                    _memberQueues[member].Dequeue();
            }
        }



        private void _checkMemberIsRegistered(Member member)
        {
            if (!_memberQueues.ContainsKey(member))
                throw new InvalidOperationException("Member is not registered.");
        }
        #endregion
    }
}