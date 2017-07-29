using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class CaseQueue
    {
        #region private fields
        private Queue<AllocatedCase> _queue;
        private Dictionary<AllocatedCase, Hour> _timeOfEnqueuing;
        #endregion

        #region internal properties
        internal int Count { get { return _queue.Count; } }
        #endregion

        #region consctructors
        internal CaseQueue()
        {
            _queue = new Queue<AllocatedCase>();
            _timeOfEnqueuing = new Dictionary<AllocatedCase, Hour>();
        }
        #endregion


        #region internal methods
        internal void Enqueue(Hour hour, AllocatedCase t)
        {
            _queue.Enqueue(t);
            _timeOfEnqueuing[t] = hour;
        }

        internal AllocatedCase Dequeue()
        {
            if (Count == 0)
                return default(AllocatedCase);

            AllocatedCase ac = _queue.Dequeue();
            _timeOfEnqueuing.Remove(ac);
            return ac;
        }

        internal AllocatedCase Peek()
        {
            return _queue.Peek();
        }
        #endregion
    }
}